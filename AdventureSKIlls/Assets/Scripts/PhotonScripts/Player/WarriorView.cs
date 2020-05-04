using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WarriorView : BaseView
{
    const string shieldKey = "SH";
    const string shieldActiveKey = "SA";

    private Vector2 lastShieldScaleSent;
    private bool lastShieldBoolSent;

    private Vector2 lastShieldScaleReceived;
    private bool lastShieldBoolReceived;


    Warrior warriorMain;
    GameObject shield;
    Transform shieldTransform;

    public override void Awake()
    {
        base.Awake();

        warriorMain = GetComponent<Warrior>();
        shield = warriorMain.shield;
        shieldTransform = shield.transform;

        lastShieldScaleReceived = shieldTransform.localScale;
        lastShieldScaleSent = shieldTransform.localScale;

        lastShieldBoolReceived = shield.activeSelf;
        lastShieldBoolSent = shield.activeSelf;
    }

    #region SerializeShield
    void StoreShield()
    {
        if(lastShieldScaleSent != (Vector2)shield.transform.localScale)
        {
            stringToSend += shieldKey + shield.transform.localScale.x + ";" + shield.transform.localScale.y + ";";
            lastShieldScaleSent = shield.transform.localScale;
        }

        if(lastShieldBoolSent != shield.activeSelf)
        {
            stringToSend += shieldActiveKey + shield.activeSelf + ";";
            lastShieldBoolSent = shield.activeSelf;
        }
    }

    void ReadShield()
    {
        if(stringReceived.Contains(shieldKey))
        {
            int shieldIndex = stringReceived.IndexOf(shieldKey);
            int newStringIndex = 0;
            string[] newScale = new string[2];
            newScale[0] = ""; newScale[1] = "";

            for(int x = shieldIndex + shieldKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    newScale[newStringIndex] += stringReceived[x];
                else if (newStringIndex != newScale.Length - 1)
                    newStringIndex++;
                else break;
            }

            lastShieldScaleReceived = new Vector2(float.Parse(newScale[0]), float.Parse(newScale[1]));
        }

        if(stringReceived.Contains(shieldActiveKey))
        {
            int shieldActiveIndex = stringReceived.IndexOf(shieldActiveKey);
            string newActive = "";

            for(int x = shieldActiveIndex + shieldActiveKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    newActive += stringReceived[x];
                else break;
            }

            lastShieldBoolReceived = bool.Parse(newActive);
        }
    }

    void UpdateShield()
    {
        if ((Vector2)shield.transform.localScale != lastShieldScaleReceived)
            shield.transform.localScale = lastShieldScaleReceived;
        if (shield.activeSelf != lastShieldBoolReceived)
            shield.SetActive(lastShieldBoolReceived);
    }
    #endregion

    public override void PrepareToSerialize()
    {
        base.PrepareToSerialize();
        StoreShield();
    }

    
    public override void UpdatePlayer()
    {
        base.UpdatePlayer();
        UpdateShield();
    }

    public override void ReadString()
    {
        base.ReadString();
        ReadShield();
    }

}
