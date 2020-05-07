using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slime : BaseStats
{

    Rigidbody2D rb;

    public float spd;

    public Vector2 wallCheckOffset, wallCheckSize;
    public Vector2 floorCheckOffset, floorCheckSize;
    public LayerMask checkMask;

    Vector2 wallCheckPos, floorCheckPos;
    public bool floorAhead, wallAhead;

    int hor;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();

        hor = Random.Range(0, 2);
        hor = hor == 1 ? 1 : -1;
        transform.right *= hor;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case BaseState.STANDARD:

                if (rb.velocity.magnitude < .1f)
                    rb.velocity = spd * transform.right;

                CheckFloorOrWall();

                break;
        }
    }

    IEnumerator Hurt()
    {
        state = BaseState.HURT;
        gameObject.layer = 9;
        yield return new WaitForSeconds(.1f);
        gameObject.layer = 12;
        state = BaseState.STANDARD;
    }

    void CheckFloorOrWall()
    {
        hor = transform.eulerAngles.y == 0 ? 1 : -1;

        floorCheckPos = (Vector2)transform.position + new Vector2(floorCheckOffset.x * hor, floorCheckOffset.y);
        wallCheckPos = (Vector2)transform.position + new Vector2(wallCheckOffset.x * hor, wallCheckOffset.y);

        floorAhead = Physics2D.OverlapBox(floorCheckPos, floorCheckSize, 0, checkMask);
        wallAhead = Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0, checkMask);

        if (!floorAhead || wallAhead)
        {
            transform.right = -transform.right;
            rb.velocity = Vector2.zero;
        }
    }

    public override void TakeDamage(float damageTaken, Collider2D col)
    {
        rb.velocity = (transform.position - col.transform.position).normalized * 10;
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

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        hor = transform.eulerAngles.y == 0 ? 1 : -1;

        floorCheckPos = (Vector2)transform.position + new Vector2(floorCheckOffset.x * hor, floorCheckOffset.y);
        wallCheckPos = (Vector2)transform.position + new Vector2(wallCheckOffset.x * hor, wallCheckOffset.y);

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(floorCheckPos, floorCheckSize);
        Gizmos.DrawWireCube(wallCheckPos, wallCheckSize);
    }
}
