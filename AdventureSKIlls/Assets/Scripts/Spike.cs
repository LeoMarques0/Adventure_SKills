using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    public float dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
            collision.GetComponent<BaseStats>().TakeDamage(dmg);
        Destroy(gameObject);
    }
}
