using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public AudioSource collect;
    Collider2D[] colliders;
    SpriteRenderer sp;

    private void Start()
    {
        colliders = GetComponents<Collider2D>();
        collect = GetComponent<AudioSource>();
        sp = GetComponent<SpriteRenderer>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }
            sp.enabled = false;
            collect.Play();
            Destroy(gameObject,1);
        }
    }
}
