using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public float dmg = 20;
    public Vector2 knockbackDir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<BaseStats>().TakeDamage(dmg, knockbackDir, true);
        }
    }
}
