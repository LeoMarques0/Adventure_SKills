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

    Animator anim;
    Collider2D hit;
    bool canAttack = true;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BaseState.STANDARD:
                if (canAttack)
                    anim.Play("Attack_Skeleton");

                hit = Physics2D.OverlapBox((Vector2)transform.position + checkOffset, checkSize, 0, playerMask);

                if(hit != null && hit.tag == "Player")
                {
                    int yRot = hit.transform.position.x - transform.position.x > 0 ? 0 : 180;
                    transform.eulerAngles = new Vector3(0, yRot, 0);
                }

                break;
        }
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

    IEnumerator Hurt()
    {
        state = BaseState.HURT;
        gameObject.layer = 9;
        yield return new WaitForSeconds(.1f);
        gameObject.layer = 11;
        state = BaseState.STANDARD;
    }

    public override void TakeDamage(float damageTaken, Collider2D col)
    {
        StartCoroutine(FlashSprite(.1f, 1));
        StartCoroutine(Hurt());

        if (!online)
        {
            health -= damageTaken;
            if (health <= 0)
                Die();
        }
        else
        {
            photonView.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damageTaken);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + checkOffset, checkSize);
    }
}
