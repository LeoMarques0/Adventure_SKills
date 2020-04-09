using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
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
        yield return new WaitForSeconds(1f);
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

    public override void TakeDamage(float damageTaken)
    {
        rb.velocity = Vector2.zero;
        StartCoroutine(Hurt());
        base.TakeDamage(damageTaken);
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
