using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    public GameObject[] players;
    public GameObject[] spotlights;

    public int playerIndex;

    GameObject currentPlayer;

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
        if (PhotonNetwork.InRoom)
        {
            switch (Input.inputString)
            {

                case "0":

                    ChangeClass(-1);

                    break;

                case "1":

                    ChangeClass(0);

                    break;

                case "2":

                    ChangeClass(1);

                    break;

                case "3":

                    ChangeClass(2);

                    break;

                case "4":

                    ChangeClass(3);

                    break;
            }
        }
    }

    void ChangeClass(int index)
    {

        if (currentPlayer != null)
            PhotonNetwork.Destroy(currentPlayer);

        for(int x = 0; x < players.Length; x++)
        {
            if (x == index)
            {
                Vector2 startPos = new Vector2(spotlights[playerIndex].transform.position.x, 0);
                currentPlayer = PhotonNetwork.Instantiate(players[x].name, startPos, Quaternion.identity);
            }
        }
    }
}
