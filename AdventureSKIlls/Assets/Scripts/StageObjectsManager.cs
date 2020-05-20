using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StageObjectsManager : MonoBehaviourPun
{

    public List<GameObject> objects;

    Player[] players;
    List<GameObject> playersObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("SearchPlayers", RpcTarget.AllBuffered);
    }

    // Update is called once per frame
    void Update()
    {
        for(int y = 0; y < objects.Count; y++)
        {
            if (objects[y] == null)
            {
                objects.Remove(objects[y]);
                y--;
                return;
            }

            bool setActive = false;
            for(int x = 0; x < playersObj.Count; x++)
            {
                if (Vector2.Distance(playersObj[x].transform.position, objects[y].transform.position) < 30)
                {
                    setActive = true;
                    break;
                }
            }
            objects[y].SetActive(setActive);
        }
    }

    [PunRPC]
    void SearchPlayers()
    {
        players = FindObjectsOfType<Player>();

        List<GameObject> currentPlayers = new List<GameObject>();
        foreach (Player p in players)
        {
            currentPlayers.Add(p.gameObject);
        }
        playersObj = currentPlayers;
    }
}
