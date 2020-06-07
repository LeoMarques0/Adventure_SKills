using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Transform parent;
    public float dmg, spd;

    Rigidbody2D rb;
    BaseStats main;
    AudioSource audioSource;

    [SerializeField]
    bool rotatesWithVelocity = false;

    [SerializeField]
    Collider2D myCol = null;
    [SerializeField]
    SpriteRenderer mySprite = null;
    [SerializeField]
    GameObject[] myChildren = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rb.velocity = new Vector2(spd, 0) * transform.right;
        main = parent.GetComponent<BaseStats>();

        Destroy(gameObject, 2);
    }

    private void Update()
    {
        if (rotatesWithVelocity)
            transform.right = rb.velocity.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
        {
            if (main != null && main.online)
            {
                if (collision.gameObject.layer == 8)
                {
                    BaseStats colStats = collision.GetComponent<BaseStats>();
                    if (colStats.photonView.IsMine)
                    {
                        colStats.TakeDamage(dmg, GetComponent<Collider2D>());
                        audioSource.clip = colStats.damageTaken[Random.Range(0, colStats.damageTaken.Length)];
                        audioSource.Play();
                    }
                }
                else if (collision.gameObject.layer == 12 && main.photonView.IsMine)
                {
                    BaseStats colStats = collision.GetComponent<BaseStats>();
                    colStats.TakeDamage(dmg, GetComponent<Collider2D>());
                    audioSource.clip = colStats.damageTaken[Random.Range(0, colStats.damageTaken.Length)];
                    audioSource.Play();
                }
            }
            else if (collision.gameObject.layer == 8 || (collision.gameObject.layer == 12 && parent.gameObject.layer != 12))
            {
                BaseStats colStats = collision.GetComponent<BaseStats>();
                colStats.TakeDamage(dmg, GetComponent<Collider2D>());
                audioSource.clip = colStats.damageTaken[Random.Range(0, colStats.damageTaken.Length)];
                audioSource.Play();
            }
            Destroy(gameObject, 3);
            Deactivate();
        }
    }

    void Deactivate()
    {
        if (mySprite != null)
            mySprite.enabled = false;
        if (myCol != null)
            myCol.enabled = false;
        if(myChildren.Length > 0)
        {
            foreach (GameObject child in myChildren)
                child.SetActive(false);
        }
    }
}
