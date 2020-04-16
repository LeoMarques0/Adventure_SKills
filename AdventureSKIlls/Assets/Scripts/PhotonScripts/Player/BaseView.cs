using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour, IPunObservable
{
    BaseStats baseStats;
    Rigidbody2D rb;

    [HideInInspector]
    public List<float> stringsToJson = new List<float>();
    [HideInInspector]
    public string serializedString;

    public virtual void Awake()
    {
        baseStats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void StoreTransform2D(bool getPos, bool getRot)
    {
        stringsToJson.Add(transform.position.x);
        stringsToJson.Add(transform.position.y);

        stringsToJson.Add(transform.eulerAngles.y);
    }

    public void UpdateTransform2D(float[] floatsReceived)
    {
        transform.position = new Vector2(floatsReceived[0], floatsReceived[1]);
        transform.eulerAngles = new Vector2(0, floatsReceived[2]);
    }

    public void StoreVelocity2D()
    {
        stringsToJson.Add(rb.velocity.x);
        stringsToJson.Add(rb.velocity.y);
    }

    public void UpdateVelocity2D(float[] floatsReceived)
    {
        rb.velocity = new Vector2(floatsReceived[3], floatsReceived[4]);
    }

    public void StoreHealth()
    {
        stringsToJson.Add(baseStats.health);
    }

    public void UpdateHealth(float[] floatsReceived)
    {
        baseStats.health = floatsReceived[0];
    }

    public virtual void PrepareToSerialize()
    {
        stringsToJson = new List<float>();

        StoreHealth();
    }

    public virtual void UpdatePlayer(float[] floatsReceived)
    {
        UpdateHealth(floatsReceived);
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            PrepareToSerialize();

            serializedString = JsonUtil.CollectionToJsonString(stringsToJson, "PlayerKey");

            stream.Serialize(ref serializedString);
        }
        else if(stream.IsReading)
        {
            stream.Serialize(ref serializedString);

            float[] floatsReceived = JsonUtil.JsonStringToArray(serializedString, "PlayerKey", str => float.Parse(str));

            UpdatePlayer(floatsReceived);
        }
    }
}
