using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AudioSource hit;
    public Animator anim;
    public BaseStats player;
    public int dmg;

    Transform parent;
    

    bool canAttack, isAttacking;
    int attackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        anim = transform.root.GetComponent<Animator>();
        hit = GetComponent<AudioSource>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        AttackInput();

        if (isAttacking && canAttack)
        {
            attackIndex = attackIndex == 1 ? 2 : 1;
            canAttack = false;
            player.state = BaseState.ATTACKING;
        }

        if (!isAttacking && 
        !(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack0") && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")))
        {
            canAttack = true;
            StopAllCoroutines();
            attackIndex = 0;
            player.state = BaseState.STANDARD;
        }

        anim.SetInteger("AttackIndex", attackIndex);

    }

    public virtual void AttackInput()
    {
        if (Input.GetButtonDown("Attack"))
        {
            StopAllCoroutines();
            StartCoroutine(AttackDelay());
        }
    }

    public IEnumerator AttackDelay()
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
        if (collision.transform != parent)
        {
            if (player.online)
            {
                if (collision.gameObject.layer == 8)
                {
                    BaseStats collisionStats = collision.GetComponent<BaseStats>();
                    if (collisionStats.photonView.IsMine)
                    {
                        collisionStats.TakeDamage(dmg, GetComponent<Collider2D>());
                        hit.Play();
                    }
                }
                else if (collision.gameObject.layer == 12 && player.photonView.IsMine)
                {
                    collision.GetComponent<BaseStats>().TakeDamage(dmg, GetComponent<Collider2D>());
                    hit.Play();
                }
            }
            else if (collision.gameObject.layer == 8 || collision.gameObject.layer == 12)
            {
                collision.GetComponent<BaseStats>().TakeDamage(dmg, GetComponent<Collider2D>());
                hit.Play();
            }
        }
    }
}
