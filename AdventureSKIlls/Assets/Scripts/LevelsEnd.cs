using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LevelsEnd : MonoBehaviour
{

    public PhotonView photonView;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            photonView.RPC("EndStage", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void EndStage()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
