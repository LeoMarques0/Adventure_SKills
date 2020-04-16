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
    public GameObject[] drops;

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
        DropItems();
        Destroy(gameObject);
    }

    public virtual void DropItems()
    {
        foreach (GameObject drop in drops)
        {
            GameObject newDrop = Instantiate(drop, transform.position, Quaternion.identity);
            Rigidbody2D dropRb = newDrop.GetComponent<Rigidbody2D>();
            Vector2 dropDir = new Vector2(Random.Range(-5, 5), 10);
            dropRb.AddForce(dropDir, ForceMode2D.Impulse);
        }
    }
}
