using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    public float abilityDelay;
    public int healAmount;

    public PlayerProjectile magic;

    bool canHeal = true;

    public override void Update()
    {
        base.Update();
        CallHeal();
    }

    void CallHeal()
    {
        if (Input.GetButtonDown("Ability") && canHeal && health < 100)
        {            
            anim.SetBool("useAbility", true);            
        }
    }

    public void Heal()
    {
        health += healAmount;
        StartCoroutine(HealDelay());
    }

    IEnumerator HealDelay()
    {
        canHeal = false;
        anim.SetBool("useAbility", canHeal);
        yield return new WaitForSeconds(abilityDelay);
        canHeal = true;
    }

    public void CallShot()
    {
        magic.Shoot();
    }
}
