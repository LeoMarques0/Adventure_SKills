using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThiefView : BaseView
{

    Thief thiefMain;

    SpriteRenderer[] sprites;
    float[] srAlphas;

    public override void Awake()
    {
        base.Awake();

        thiefMain = GetComponent<Thief>();

        sprites = thiefMain.sprites;
        srAlphas = new float[sprites.Length];
        srAlphas[0] = sprites[0].color.a;
    }

    public override void PrepareToSerialize()
    {
        base.PrepareToSerialize();

        if (srAlphas[0] != sprites[0].color.a)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                srAlphas[i] = sprites[i].color.a;
                stringsToJson.Add(srAlphas[i]);
            }
        }
    }

    /*
    public override void UpdatePlayer()
    {
        base.UpdatePlayer();

        if (floatsReceived.Length > 5)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                srAlphas[i] = floatsReceived[1 + i];

                Color spriteColor = sprites[i].color;
                spriteColor.a = srAlphas[i];
                sprites[i].color = spriteColor;
            }
        }
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            PrepareToSerialize();            

            serializedString = JsonUtil.CollectionToJsonString(stringsToJson, "ThiefKey");

            stream.Serialize(ref serializedString);
        }
        else if(stream.IsReading)
        {

            stream.Serialize(ref serializedString);

            float[] floatsReceived = JsonUtil.JsonStringToArray(serializedString, "ThiefKey", str => float.Parse(str));

            UpdatePlayer(floatsReceived);            
        }
    }
    */
}
