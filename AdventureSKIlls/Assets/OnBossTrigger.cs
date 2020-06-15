using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBossTrigger : MonoBehaviour
{
    AudioSource audios;
    public AudioClip lastmusic;

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audios.clip = lastmusic;
            audios.PlayOneShot(audios.clip);
        }
    }
}
