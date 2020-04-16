using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float delay;
    public Animator animator;

    bool canAttack = true;
    bool attack;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Attack") && canAttack)
        {
            attack=true;
            animator.SetBool("Attacking?", attack);
            Projectile newShot = Instantiate(projectile, transform.position, transform.parent.rotation).GetComponent<Projectile>();
            newShot.parent = transform.parent;

            StartCoroutine(ShotDelay());
        }
    }

    IEnumerator ShotDelay()
    {
        canAttack = false;
        attack = false;
        animator.SetBool("Attacking?", attack);
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }
}
