using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int dmg;

    Transform parent;
    Collider2D col;
    SpriteRenderer sr;

    bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            StartCoroutine(AttackTime());
        }

    }

    IEnumerator AttackTime()
    {
        col.enabled = true;
        sr.enabled = true;
        canAttack = false;

        yield return new WaitForSeconds(.5f);

        col.enabled = false;
        sr.enabled = false;
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform != parent)
        {
            print(collision.name);
            collision.GetComponent<BaseStats>().TakeDamage(dmg);
        }
    }
}
