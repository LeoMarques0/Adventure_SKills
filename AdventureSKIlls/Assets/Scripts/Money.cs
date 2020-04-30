using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public AudioSource collect;
    SpriteRenderer sp;

    private void Start()
    {
        collect = transform.parent.GetComponent<AudioSource>();
        sp = GetComponent<SpriteRenderer>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collect.Play();
            Destroy(transform.parent.gameObject, 5);
            Destroy(gameObject);
        }
    }
}
