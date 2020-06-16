using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public AudioSource collect;
    SpriteRenderer sp;
    StageManager stageManager;

    private void Start()
    {
        collect = transform.parent.GetComponent<AudioSource>();
        sp = GetComponent<SpriteRenderer>();

        stageManager = FindObjectOfType<StageManager>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player collidedWith = collision.GetComponent<Player>();
            if (collidedWith.health > 0)
            {
                if (collidedWith.photonView.IsMine)
                {
                    collidedWith.coins++;
                    stageManager.CallUpdateCoin();
                }
                collect.Play();
                Destroy(transform.parent.gameObject, 5);
                Destroy(gameObject);
            }
        }
    }
}
