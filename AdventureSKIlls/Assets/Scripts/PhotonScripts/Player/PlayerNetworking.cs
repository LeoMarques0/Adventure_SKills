using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] scritpsToIgnore;

    [HideInInspector]
    public PhotonView photonView;

    // Start is called before the first frame update
    public virtual void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(!photonView.IsMine)
        {
            foreach(MonoBehaviour script in scritpsToIgnore)
            {
                script.enabled = false;
            }
        }
    }
}
