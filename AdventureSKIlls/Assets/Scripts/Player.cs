using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseStats
{
    int hor;

    public float spd, maxSpd;

    public float jumpHeight, fallMultiplier;
    [HideInInspector]
    public float jumpForce, gravity;

    public bool instantSpeed;

    public Vector2 groundOffset, groundCheckSize;
    public LayerMask checkMask;

    [HideInInspector]
    public bool isGrounded, isJumping;

    [HideInInspector]
    public Rigidbody2D rb;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravity = Physics2D.gravity.y * rb.gravityScale;
        jumpForce = Mathf.Sqrt(-2 * gravity * jumpHeight);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        switch(state)
        {
            case BaseState.STANDARD:

                Walk();
                Jump();

                break;

            case BaseState.HURT:

                gameObject.layer = 9;

                break;
        }

        CollisionsCheck();

        health = Mathf.Clamp(health, 0, 100);
    }

    void Walk()
    {
        hor = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if(instantSpeed)
            rb.velocity = new Vector2(maxSpd * hor, rb.velocity.y);
        else
        {
            if ((hor > 0 && rb.velocity.x < 0) || (hor < 0 && rb.velocity.x > 0))
                rb.AddForce(Vector2.right * hor * spd * 5 * Time.deltaTime);
            else
                rb.AddForce(Vector2.right * hor * spd * Time.deltaTime);

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpd, maxSpd), rb.velocity.y);

            if (hor == 0)
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0, rb.velocity.y), 8 * Time.deltaTime);
        }
    }

    public virtual void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            isJumping = false;
        }
        else if(rb.velocity.y > .5f)
            isJumping = true;
    }

    public virtual void CollisionsCheck()
    {
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + groundOffset, groundCheckSize, 0, checkMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundOffset, groundCheckSize);
    }
}
