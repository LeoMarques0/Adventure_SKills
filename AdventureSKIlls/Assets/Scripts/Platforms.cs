using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Platforms : MonoBehaviour
{

    Player[] players;
    GameObject myPlayer;

    private void Start()
    {
        players = FindObjectsOfType<Player>();
        if (players.Length > 1)
        {
            foreach (Player player in players)
            {
                if (GameManager.singleton.currentPlayer == player.gameObject)
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
