using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StageManager : MonoBehaviour
{

    public Transform[] playersSpawns;

    // Start is called before the first frame update
    void Start()
    {
        int classIndex = GameManager.singleton.playerCharacterIndex;
        PhotonNetwork.Instantiate(GameManager.singleton.players[classIndex].name, playersSpawns[classIndex].position, Quaternion.identity);
    }
}
