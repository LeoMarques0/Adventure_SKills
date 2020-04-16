using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float delay;
    public Animator anim;

    public BaseStats player;

    private void Start()
    {
        anim = transform.root.GetComponent<Animator>();
        //player = transform.root.GetComponent<BaseStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            StartCoroutine(Attack());
        }
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && player.state == BaseState.ATTACKING)
            player.state = BaseState.STANDARD;
    }

    IEnumerator Attack()
    {
        anim.SetBool("Attacking?", true);
        yield return new WaitForSeconds(.2f);
        player.state = BaseState.ATTACKING;
    }

    public void Shoot()
    {
        Projectile newShot = Instantiate(projectile, transform.position, transform.parent.rotation).GetComponent<Projectile>();
        newShot.parent = transform.root;
        anim.SetBool("Attacking?", false);
    }
}
