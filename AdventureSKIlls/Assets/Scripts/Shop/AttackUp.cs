using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : Upgrade
{
    public override void UpgradeEffect()
    {
        base.UpgradeEffect();
        GameManager.singleton.currentPlayer.GetComponent<BaseStats>().additionalDamage += 1;
        GameManager.singleton.BuyUpgrade("ATTACK");
    }
}
