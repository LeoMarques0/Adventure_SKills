using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int dmg;

    Transform parent;
    BaseStats player;
    Animator anim;

    bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        anim = transform.root.GetComponent<Animator>();
        player = transform.root.GetComponent<BaseStats>();

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            StartCoroutine(Attack());
        }
        else
            anim.SetBool("Attacking?", false);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && player.state == BaseState.ATTACKING)
            player.state = BaseState.STANDARD;

    }

    IEnumerator Attack()
    {
        anim.SetBool("Attacking?", true);
        yield return new WaitForSeconds(.05f);
        player.state = BaseState.ATTACKING;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform != parent && (collision.gameObject.layer == 8 || collision.gameObject.layer == 12))
        {
            print(collision.name);
            collision.GetComponent<BaseStats>().TakeDamage(dmg);
        }
    }
}
