using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    public MonoBehaviour[] scritpsToIgnore;

    [HideInInspector]
    public PhotonView photonView;

    public virtual void OnEnable()
    {
        print("Check");
        if (!PhotonNetwork.InRoom)
        {
            print("Got Here");
            return;
        }
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            foreach (MonoBehaviour script in scritpsToIgnore)
            {
                script.enabled = false;
            }
        }
        else
        {
            GameManager.singleton.currentPlayer = gameObject;
        }
    }
}
