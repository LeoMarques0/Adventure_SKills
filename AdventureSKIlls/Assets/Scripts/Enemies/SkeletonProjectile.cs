using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonProjectile : Projectile
{

    private float gravity = -9.81f;
    private Rigidbody2D rb;

    public float xVel, yVel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = Physics2D.gravity.y * rb.gravityScale;

        Vector2 throwVel = new Vector2(parent.right.x * xVel, yVel);
        rb.velocity = throwVel;

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 previousPos = transform.position;
        for(float x = 0; x < 20; x += .1f)
        {
            if (x > 0)
            {
                Vector2 newPos = new Vector2(xVel * x, (yVel * x) + (.5f * -9.81f * 3 * Mathf.Pow(x, 2)));
                Gizmos.DrawLine(previousPos, newPos);
                previousPos = newPos;
            }
        }
    }
}
