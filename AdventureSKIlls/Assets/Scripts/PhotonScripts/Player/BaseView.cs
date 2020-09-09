using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour, IPunObservable
{

    const string healthKey = "HE";
    const string xPosKey = "XP", yPosKey = "YP";
    const string yRotKey = "YR";

    private float lastHealthSent;
    private float lastXPosSent, lastYPosSent;
    private float lastYRotSent;

    private float lastHealthReceived;
    private float lastXPosReceived, lastYPosReceived;
    private float lastYRotReceived;

    [HideInInspector]
    public string stringToSend = "";
    [HideInInspector]
    public string stringReceived;

    [SerializeField]
    private float lerpTransformTime;
    [SerializeField]
    private float lerpRotationTime;

    BaseStats baseStats;
    Rigidbody2D rb;

    [HideInInspector]
    public List<float> stringsToJson = new List<float>();
    [HideInInspector]
    public string serializedString;

    public bool sharePosition;
    public bool shareRotation;

    public virtual void Awake()
    {
        baseStats = GetComponent<BaseStats>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastHealthSent = baseStats.health;
        lastXPosSent = transform.position.x;
        lastYPosSent = transform.position.y;
        lastYRotSent = transform.eulerAngles.y;

        lastHealthReceived = baseStats.maxHealth;
        lastXPosReceived = transform.position.x;
        lastYPosReceived = transform.position.y;
        lastYRotReceived = transform.eulerAngles.y;

        transform.position = new Vector2(lastXPosSent, lastYPosSent);
        transform.eulerAngles = new Vector2(transform.eulerAngles.x, lastYRotSent);
        baseStats.health = lastHealthSent;
    }

    public virtual void Update()
    {
        if(!baseStats.photonView.IsMine)
            UpdatePlayer();
    }

    #region SerializePosition
    public void StorePosition()
    {
        if (transform.position.x != lastXPosSent)
        {
            stringToSend += xPosKey + transform.position.x + ";";
            lastXPosSent = transform.position.x;
        }
        if (transform.position.y != lastYPosSent)
        {
            stringToSend += yPosKey + transform.position.y + ";";
            lastYPosSent = transform.position.y;
        }
    }

    public void ReadPosition()
    {
        if(stringReceived.Contains(xPosKey))
        {
            int xPosIndex = stringReceived.IndexOf(xPosKey);
            string xNewPos = "";

            for(int x = xPosIndex + xPosKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    xNewPos += stringReceived[x];
                else
                    break;
            }

            lastXPosReceived = float.Parse(xNewPos);
        }

        if (stringReceived.Contains(yPosKey))
        {
            int yPosIndex = stringReceived.IndexOf(yPosKey);
            string yNewPos = "";

            for (int x = yPosIndex + yPosKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    yNewPos += stringReceived[x];
                else
                    break;
            }

            lastYPosReceived = float.Parse(yNewPos);
        }
    }

    public void UpdatePosition()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;

        if (transform.position.x != lastXPosReceived)
        {
            xPos = lastXPosReceived;

        }

        if (transform.position.y != lastYPosReceived)
        {

            yPos = lastYPosReceived;

        }

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(xPos, yPos), lerpTransformTime * Time.deltaTime);
    }
    #endregion
    #region SerializeRotation
    public void StoreRotation()
    {
        if (transform.eulerAngles.y != lastYPosSent)
        {
            stringToSend += yRotKey + transform.eulerAngles.y + ";";
            lastYRotSent = transform.eulerAngles.y;
        }
    }

    public void ReadRotation()
    {
        if (stringReceived.Contains(yRotKey))
        {
            int yRotIndex = stringReceived.IndexOf(yRotKey);
            string yNewRot = "";

            for (int x = yRotIndex + yRotKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    yNewRot += stringReceived[x];
                else
                    break;
            }

            lastYRotReceived = float.Parse(yNewRot);
        }
    }

    public void UpdateRotation()
    {
        float yRot = transform.eulerAngles.y;
        if (transform.eulerAngles.y != lastYRotReceived)
        {
            if (lerpTransformTime == 0)
                yRot = lastYRotReceived;
            else
                yRot = Mathf.Lerp(yRot, lastYRotReceived, lerpTransformTime);
        }

        transform.eulerAngles = new Vector2(transform.eulerAngles.x, yRot);
    }
    #endregion
    #region SerializeHealth
    public void StoreHealth()
    {
        if (baseStats.health != lastHealthSent)
        {
            stringToSend += healthKey + baseStats.health + ";";
            lastHealthSent = baseStats.health;
        }
    }

    public void ReadHealth()
    {
        if (stringReceived.Contains(healthKey))
        {
            int healthIndex = stringReceived.IndexOf(healthKey);
            string newHealth = "";

            for (int x = healthIndex + healthKey.Length; x < stringReceived.Length; x++)
            {
                if (stringReceived[x] != ';')
                    newHealth += stringReceived[x];
                else
                    break;
            }

            lastHealthReceived = float.Parse(newHealth);
        }
    }

    public void UpdateHealth()
    {
        if(baseStats.health != lastHealthReceived)
            baseStats.health = lastHealthReceived;
    }
    #endregion
    #region SerializeManager
    public virtual void PrepareToSerialize()
    {
        StoreHealth();
        if(sharePosition)
            StorePosition();
        if(shareRotation)
            StoreRotation();
    }

    public virtual void ReadString()
    {
        ReadHealth();
        if(sharePosition)
            ReadPosition();
        if (shareRotation)
            ReadRotation();
    }

    public virtual void UpdatePlayer()
    {
        UpdateHealth();
        if(sharePosition)
            UpdatePosition();
        if (shareRotation)
            UpdateRotation();
    }
    #endregion

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            PrepareToSerialize();

            serializedString = stringToSend;
            stringToSend = "";

            stream.SendNext(serializedString);
        }
        else
        {
            stringReceived = (string)stream.ReceiveNext();

            ReadString();
        }
    }
}
