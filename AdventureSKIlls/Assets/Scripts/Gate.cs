using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : BaseStats
{

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && collision.transform.parent.name.Contains("Warrior"))
        {
            TakeDamage(7, Vector2.zero, false);
            StartCoroutine(FlashSprite(.1f, 1));
        }
    }
}
