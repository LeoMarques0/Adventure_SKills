using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeSegment : MonoBehaviourPun
{

    HingeJoint2D hinge;

    private void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            photonView.RPC("DisableHinge", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void DisableHinge()
    {
        hinge.enabled = false;
    }
}
