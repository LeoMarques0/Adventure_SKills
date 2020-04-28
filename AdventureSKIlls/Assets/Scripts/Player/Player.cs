﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseStats
{
    int hor;

    Collider2D currentGround;
    PlayerAttack attacks;

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
    [HideInInspector]
    public Animator anim;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attacks = transform.GetComponentInChildren<PlayerAttack>();

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

            case BaseState.ATTACKING:

                rb.velocity = new Vector2(0, rb.velocity.y);

                break;
        }

        CollisionsCheck();

        health = Mathf.Clamp(health, 0, 100);

        anim.SetFloat("Speed", Mathf.Abs(hor));
        anim.SetBool("IsGrounded", isGrounded);
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

        if(hor != 0)
            transform.eulerAngles = Vector3.up * (hor == 1 ? 1 : 180);
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
        {
            isJumping = true;
        }

        anim.SetFloat("YSpeed", rb.velocity.y);

    }

    public virtual void CollisionsCheck()
    {
        currentGround = Physics2D.OverlapBox((Vector2)transform.position + groundOffset, groundCheckSize, 0, checkMask);
        if (currentGround)
            transform.parent = currentGround.transform;
        else
            transform.parent = null;
        isGrounded = currentGround;
    }

    public override void Die()
    {
        base.Die();
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.Play("Dying");
    }

    public void EndAttack()
    {
        attacks.EndAttack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundOffset, groundCheckSize);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Espinhos"))
        {
            TakeDamage(100);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            TakeDamage(20);
        }
    }
}