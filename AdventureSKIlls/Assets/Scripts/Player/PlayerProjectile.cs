using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float delay;

    bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Attack") && canAttack)
        {
            Projectile newShot = Instantiate(projectile, transform.position, transform.parent.rotation).GetComponent<Projectile>();
            newShot.parent = transform.parent;

            StartCoroutine(ShotDelay());
        }
    }

    IEnumerator ShotDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }
}
