using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Platforms : MonoBehaviour
{

    PlayerNetworking[] players;
    GameObject myPlayer;

    private void Start()
    {
        players = FindObjectsOfType<PlayerNetworking>();
        if (players.Length > 1)
        {
            foreach (PlayerNetworking player in players)
            {
                if (player.photonView.IsMine)
                {
                    myPlayer = player.gameObject;
                    break;
                }
            }
        }
        else
            myPlayer = players[0].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer.transform.position.y - transform.position.y < 0)
            gameObject.layer = 15;
        else
            gameObject.layer = 14;
    }
}
