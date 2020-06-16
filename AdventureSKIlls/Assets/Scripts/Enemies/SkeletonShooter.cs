using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SkeletonShooter : BaseStats
{

    public LayerMask playerMask;
    public Vector2 checkOffset;
    public Vector2 checkSize;
    public GameObject projectile;
    public Transform shotPos;

    public float delay;
    public float spd;

    Animator anim;
    Collider2D hit;
    Rigidbody2D rb;
    bool canAttack = true;
    bool isSearching = false;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.OverlapBox((Vector2)transform.position + checkOffset, checkSize, 0, playerMask);
        switch (state)
        {
            case BaseState.STANDARD:
               
                if(hit != null && hit.tag == "Player")
                {
                    int yRot = hit.transform.position.x - transform.position.x > 0 ? 0 : 180;
                    transform.eulerAngles = new Vector3(0, yRot, 0);
                    state = BaseState.ATTACKING;
                }

                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("isAttacking", false);

                break;

            case BaseState.ATTACKING:

                if (hit != null)
                {
                    int yRot = hit.transform.position.x - transform.position.x > 0 ? 0 : 180;
                    transform.eulerAngles = new Vector3(0, yRot, 0);

                    if (canAttack)
                        anim.SetBool("isAttacking", true);
                    else
                    {
                        anim.SetBool("isAttacking", false);

                        float hitDistance = Mathf.Abs(hit.transform.position.x - transform.position.x);
                        if (hitDistance < 8 || (rb.velocity.x == 0 && hitDistance > 8 && hitDistance < 10))
                            rb.velocity = new Vector2(-transform.right.x * spd, rb.velocity.y);
                        else if (hitDistance > 12 || (rb.velocity.x == 0 && hitDistance > 10 && hitDistance < 12))
                            rb.velocity = new Vector2(transform.right.x * spd, rb.velocity.y);
                        else
                            rb.velocity = new Vector2(0, rb.velocity.y);
                    }

                    if(isSearching)
                        StopCoroutine("SearchPlayer");                 
                }
                else if(hit == null && !isSearching)
                {
                    anim.SetBool("isAttacking", false);
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    StartCoroutine(SearchPlayer());
                }

                break;
        }
        anim.SetFloat("Speed", rb.velocity.x * (transform.eulerAngles.y == 0 ? 1 : -1));
    }

    public void Shoot()
    {
        Projectile newShot = Instantiate(projectile, shotPos.position, transform.rotation).GetComponent<Projectile>();
        newShot.parent = transform;

        StartCoroutine(ShotDelay());
    }

    IEnumerator ShotDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

    IEnumerator SearchPlayer()
    {
        isSearching = true;
        for(int x = 0; x < 2; x++)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(.25f);
            transform.eulerAngles = new Vector3(0, 180, 0);
            yield return new WaitForSeconds(.25f);
        }
        state = BaseState.STANDARD;
    }

    IEnumerator Hurt()
    {
        state = BaseState.HURT;
        gameObject.layer = 9;
        yield return new WaitForSeconds(.1f);
        gameObject.layer = 12;
        state = BaseState.STANDARD;
    }

    public override void TakeDamage(float damageTaken, Vector2 dir, bool localDir)
    {
        StartCoroutine(Hurt());

        if (!online)
        {
            health -= damageTaken;
            if (health <= 0)
                Die();
        }
        else
        {
            photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damageTaken, dir, localDir);
        }
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + checkOffset, checkSize);
    }
}
