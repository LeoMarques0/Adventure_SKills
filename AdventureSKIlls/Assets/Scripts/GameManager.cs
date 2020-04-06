using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    public GameObject[] players;

    public int playerIndex;

    public GameObject currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeClass(string className, Vector2 startPos)
    {
        if (currentPlayer != null)
            PhotonNetwork.Destroy(currentPlayer);

        currentPlayer = PhotonNetwork.Instantiate(className, startPos, Quaternion.identity);
    }

    public void DeleteClass()
    {
        PhotonNetwork.Destroy(currentPlayer);
    }
}
