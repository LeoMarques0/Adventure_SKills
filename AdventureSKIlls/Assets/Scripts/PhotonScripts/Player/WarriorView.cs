using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WarriorView : PlayerView
{

    Warrior warriorMain;
    GameObject shield;
    Transform shieldTransform;

    public override void Awake()
    {
        base.Awake();

        warriorMain = GetComponent<Warrior>();
        shield = warriorMain.shield;
        shieldTransform = shield.transform;
    }

    public override void PrepareToSerialize()
    {
        base.PrepareToSerialize();

        stringsToJson.Add(shield.activeSelf ? 1 : 0);

        stringsToJson.Add(shieldTransform.localScale.x);
        stringsToJson.Add(shieldTransform.localScale.y);
    }

    public override void UpdatePlayer(float[] floatsReceived)
    {
        base.UpdatePlayer(floatsReceived);

        shield.SetActive(floatsReceived[1] > 0 ? true : false);

        Vector2 shieldScale = shieldTransform.localScale;

        shieldScale.x = floatsReceived[2];
        shieldScale.y = floatsReceived[3];

        shieldTransform.localScale = shieldScale;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);
    }
}
