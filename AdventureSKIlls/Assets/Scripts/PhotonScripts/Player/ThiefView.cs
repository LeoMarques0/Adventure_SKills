using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThiefView : BaseView
{

    const string invicibilityKey = "IK";
    const string stateKey = "SK";

    Thief thiefMain;

    SpriteRenderer[] sprites;
    float[] srAlphas;

    private BaseState lastStateSent = BaseState.STANDARD;
    private BaseState lastStateReceived = BaseState.STANDARD;

    private bool lastInvicibilitySent = false;
    private bool lastInvicibilityReceived = false;

    public override void Awake()
    {
        base.Awake();

        thiefMain = GetComponent<Thief>();

        sprites = thiefMain.sprites;
        srAlphas = new float[sprites.Length];
        srAlphas[0] = sprites[0].color.a;
    }

    public override void Update()
    {
        base.Update();

        if(!thiefMain.photonView.IsMine)
        {
            if (thiefMain.usingInvisibility)
                thiefMain.ChangeTransparency();
            else
                thiefMain.DisableTransparency();
        }
    }

    #region Invicibility

    private void StoreInvicibility()
    {
        if(thiefMain.usingInvisibility != lastInvicibilitySent)
        {
            stringToSend += invicibilityKey + thiefMain.usingInvisibility.ToString() + ";";
            lastInvicibilitySent = thiefMain.usingInvisibility;
        }
    }

    private void ReadInvicibility()
    {
        if(stringReceived.Contains(invicibilityKey))
        {
            int invicibilityIndex = stringReceived.IndexOf(invicibilityKey);
            string newValue = "";

            for (int x = invicibilityIndex + invicibilityKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    newValue += stringReceived[x];
                else break;
            }

            lastInvicibilityReceived = newValue == "True" ? true : false;
        }
    }

    private void UpdateInvicivility()
    {
        if (thiefMain.usingInvisibility != lastInvicibilityReceived)
            thiefMain.usingInvisibility = lastInvicibilityReceived;
    }

    #endregion

    #region State

    private void StoreState()
    {
        if(thiefMain.state != lastStateSent)
        {
            stringToSend += stateKey + (int)thiefMain.state + ";";
            lastStateSent = thiefMain.state;
        }
    }

    private void ReadState()
    {
        if(stringReceived.Contains(stateKey))
        {
            int stateIndex = stringReceived.IndexOf(stateKey);
            string newState = "";

            for (int x = stateIndex + stateKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    newState += stringReceived[x];
                else break;
            }

            lastStateReceived = (BaseState)int.Parse(newState);
        }
    }

    private void UpdateState()
    {
        if (thiefMain.state != lastStateReceived)
            thiefMain.state = lastStateReceived;
    }

    #endregion

    public override void PrepareToSerialize()
    {
        base.PrepareToSerialize();
        StoreInvicibility();
        StoreState();
    }

    
    public override void UpdatePlayer()
    {
        base.UpdatePlayer();
        UpdateInvicivility();
        UpdateState();
    }

    public override void ReadString()
    {
        base.ReadString();
        ReadInvicibility();
        ReadState();
    }
    
}
