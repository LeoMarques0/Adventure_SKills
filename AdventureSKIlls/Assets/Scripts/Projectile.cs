using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Transform parent;
    public float dmg, spd;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(spd, 0) * transform.right;

        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
        {
            print(collision.name);
            if(collision.gameObject.layer == 8 || (collision.gameObject.layer == 11 && parent.gameObject.layer != 11))
                collision.GetComponent<BaseStats>().TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
}
