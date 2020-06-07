using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private Transform parent = null;
    [SerializeField]
    private int dmg = 10;

    private Collider2D myCol;
    private AudioSource audioSource;

    

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Collider2D>(out myCol);
        TryGetComponent<AudioSource>(out audioSource);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            BaseStats collisionStats = collision.GetComponent<BaseStats>();
            if (collisionStats.photonView.IsMine)
            {
                collisionStats.TakeDamage(dmg, myCol);
                audioSource.clip = collisionStats.damageTaken[Random.Range(0, collisionStats.damageTaken.Length)];
                audioSource.Play();
            }
        }
    }
}
