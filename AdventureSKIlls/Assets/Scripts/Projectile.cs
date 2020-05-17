using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Transform parent;
    public float dmg, spd;

    Rigidbody2D rb;
    BaseStats main;

    [SerializeField]
    bool rotatesWithVelocity = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(spd, 0) * transform.right;
        main = parent.GetComponent<BaseStats>();

        Destroy(gameObject, 2);
    }

    private void Update()
    {
        if (rotatesWithVelocity)
            transform.right = rb.velocity.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
        {
            if (main != null && main.online)
            {
                if(collision.gameObject.layer == 8)
                {
                    BaseStats colStats = collision.GetComponent<BaseStats>();
                    if (colStats.photonView.IsMine)
                        colStats.TakeDamage(dmg, GetComponent<Collider2D>());
                }
                else if(collision.gameObject.layer == 12 && main.photonView.IsMine)
                    collision.GetComponent<BaseStats>().TakeDamage(dmg, GetComponent<Collider2D>());
            }
            else if (collision.gameObject.layer == 8 || (collision.gameObject.layer == 12 && parent.gameObject.layer != 12))
                collision.GetComponent<BaseStats>().TakeDamage(dmg, GetComponent<Collider2D>());
            Destroy(gameObject);
        }
    }
}
