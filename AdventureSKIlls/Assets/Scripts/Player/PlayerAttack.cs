using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int dmg;

    Transform parent;
    BaseStats player;
    Animator anim;
    public AudioSource hit;

    bool canAttack, isAttacking;
    int attackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        anim = transform.root.GetComponent<Animator>();
        player = transform.root.GetComponent<BaseStats>();
        hit = GetComponent<AudioSource>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Attack"))
        {
            StopAllCoroutines();
            StartCoroutine(AttackDelay());
        }

        if (isAttacking && canAttack)
        {
            attackIndex = attackIndex == 1 ? 2 : 1;
            canAttack = false;
            player.state = BaseState.ATTACKING;
        }

        if (canAttack && !isAttacking && 
        !(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack0") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            StopAllCoroutines();
            attackIndex = 0;
            player.state = BaseState.STANDARD;
        }

        anim.SetInteger("AttackIndex", attackIndex);

    }

    IEnumerator AttackDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.05f);
        isAttacking = false;
    }

    public void EndAttack()
    {
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform != parent && (collision.gameObject.layer == 8 || collision.gameObject.layer == 12))
        {
            collision.GetComponent<BaseStats>().TakeDamage(dmg);
            hit.Play();
        }
    }
}
