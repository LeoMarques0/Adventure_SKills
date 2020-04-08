using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : BaseStats
{

    public GameObject projectile;
    public Transform shotPos;
    public float delay;

    bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BaseState.STANDARD:
                if (canAttack)
                    Shoot();
                break;
        }
    }

    void Shoot()
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
        yield return new WaitForSeconds(1f);
        gameObject.layer = 11;
        state = BaseState.STANDARD;
    }

    public override void TakeDamage(float damageTaken)
    {
        StartCoroutine(Hurt());
        base.TakeDamage(damageTaken);
    }
}
