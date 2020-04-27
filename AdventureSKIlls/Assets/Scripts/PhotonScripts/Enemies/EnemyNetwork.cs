using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyNetwork : MonoBehaviour
{

    public MonoBehaviour[] scritpsToIgnore;

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            foreach (MonoBehaviour script in scritpsToIgnore)
            {
                script.enabled = false;
            }
        }
    }
}
