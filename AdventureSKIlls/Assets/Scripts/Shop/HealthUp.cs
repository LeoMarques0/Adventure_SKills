using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUp : Upgrade
{
    public override void UpgradeEffect()
    {
        base.UpgradeEffect();

        BaseStats player = GameManager.singleton.currentPlayer.GetComponent<BaseStats>();

        player.maxHealth += 5;
        player.healthInstance.GetComponent<Slider>().maxValue = player.maxHealth;
        player.health += 5;
        GameManager.singleton.BuyUpgrade("HEALTH");
    }
}
