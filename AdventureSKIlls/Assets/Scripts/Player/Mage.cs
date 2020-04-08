using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    public float abilityDelay;
    public int healAmount;

    bool canHeal = true;

    public override void Update()
    {
        base.Update();
        Heal();
    }

    void Heal()
    {
        if (Input.GetButtonDown("Ability") && canHeal && health < 100)
        {
            health += healAmount;
            StartCoroutine(HealDelay());
        }
    }

    IEnumerator HealDelay()
    {
        canHeal = false;

        yield return new WaitForSeconds(abilityDelay);

        canHeal = true;
    }
}
