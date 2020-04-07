using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaseState
{
    STANDARD,
    ATTACKING,
    HURT,
    DYING
}
public class BaseStats : MonoBehaviour
{

    [Range(0, 100)]
    public float health = 100;
    public BaseState state = new BaseState();

    public virtual void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
            Die();
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = BaseState.DYING;
        Destroy(gameObject);
    }
}
