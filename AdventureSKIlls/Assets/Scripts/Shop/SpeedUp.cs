using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Upgrade
{
    public override void UpgradeEffect()
    {
        Player player = GameManager.singleton.currentPlayer.GetComponent<Player>();

        player.maxSpd += player.maxSpd * .1f;
        GameManager.singleton.BuyUpgrade("SPEED");
    }
}
