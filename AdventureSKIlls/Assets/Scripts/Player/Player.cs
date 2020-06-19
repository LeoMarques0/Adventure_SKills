using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseStats
{
    int hor;

    Collider2D currentGround;
    PlayerAttack attacks;
    PlayerInput playerInput;
    Vector2 startScale;
    public AudioSource hit;
    public AudioClip[] hitAir;

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

    [HideInInspector]
    public int coins;
    public Sprite playerIcon;

    public Pause pauseMenu;
    public bool isPaused;

    public GameObject coin;

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attacks = transform.GetComponentInChildren<PlayerAttack>();
        playerInput = PlayerInput.singleton;

        gravity = Physics2D.gravity.y * rb.gravityScale;
        jumpForce = Mathf.Sqrt(-2 * gravity * jumpHeight);

        startScale = transform.lossyScale;

        pauseMenu = FindObjectOfType<Pause>();
        if (pauseMenu != null && photonView.IsMine)
        {
            pauseMenu.myPlayer = this;
            pauseMenu.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isPaused)
        {
            switch (state)
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
                case BaseState.DYING:

                    rb.velocity = new Vector2(0, rb.velocity.y);
                    gameObject.layer = 8;

                    break;
            }

            CollisionsCheck();

            health = Mathf.Clamp(health, 0, 100);

            anim.SetFloat("Speed", Mathf.Abs(hor));
            anim.SetBool("IsGrounded", isGrounded);

            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            anim.SetBool("IsAttacking", (currentState.IsName("Attack0") || currentState.IsName("Attack1") || currentState.IsName("AttackAir")));
        }

        if(Input.GetKeyDown(KeyCode.Escape) && pauseMenu != null)
        {
            isPaused = !isPaused;
            pauseMenu.gameObject.SetActive(isPaused);
        }
    }

    void Walk()
    {
        hor = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        rb.velocity = new Vector2(maxSpd * hor, rb.velocity.y);

        if(hor != 0)
            transform.eulerAngles = Vector3.up * (hor == 1 ? 0 : 180);
    }

    public virtual void Jump()
    {
        if(playerInput.jumpButton && isGrounded)
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
        if (photonView.IsMine)
        {
            StopAllCoroutines();
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("IsDead", true);
            state = BaseState.DYING;

            foreach (Material m in materials)
            {
                m.SetFloat("_FlashTransparency", 0);
            }

            photonView.RPC("DropPlayerItems", RpcTarget.AllBuffered, coins);
        }
    }

    public void EndAttack()
    {
        attacks.EndAttack();
        if(hitAir.Length > 0)
            hit.clip = hitAir[Random.Range(0,3)];
    }

    public override void TakeDamage(float damageTaken, Vector2 dir, bool localDir)
    {
        if (state != BaseState.HURT && state != BaseState.DYING)
        {
            StartCoroutine(Knockback(dir, localDir));
            base.TakeDamage(damageTaken, dir, localDir);
        }
    }

    IEnumerator Knockback(Vector2 dir, bool localDir)
    {
        state = BaseState.HURT;
        gameObject.layer = 9;
        if (!localDir)
            rb.velocity = dir;
        else
            rb.velocity = (transform.right * dir.x) + (transform.up * dir.y);

        yield return new WaitForSeconds(.1f);
        gameObject.layer = 8;
        if (state != BaseState.DYING && health > 0)
            state = BaseState.STANDARD;
    }

    public void OnAttack()
    {
        if(hit != null)
        {
            hit.Play();
        }
    }

    [PunRPC]
    public void DropPlayerItems(int coinsAmount)
    {
        state = BaseState.DYING;
        health = 0;
        for(int x = 0; x < coinsAmount; x++)
        {
            GameObject newDrop = Instantiate(coin, transform.position + (Vector3.up * .2f), Quaternion.identity);
            Rigidbody2D dropRb = newDrop.GetComponent<Rigidbody2D>();
            Vector2 dropDir = new Vector2(Random.Range(-5, 5), 10);
            dropRb.AddForce(dropDir, ForceMode2D.Impulse);
        }
        coins = 0;

        FindObjectOfType<StageManager>().CallUpdateCoin();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundOffset, groundCheckSize);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {            
            TakeDamage(20, Vector2.left * 2, true);
        }
    }
}
