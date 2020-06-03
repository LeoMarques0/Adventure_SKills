using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip[] audioClipArray;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skeleton"))
        {
            audioS.clip = audioClipArray[Random.Range(0,1)];
            audioS.PlayOneShot(audioS.clip);
        }
        if (collision.CompareTag("Slime"))
        {
            audioS.clip = audioClipArray[Random.Range(1, 6)];
            audioS.PlayOneShot(audioS.clip);
        }
    }

}
