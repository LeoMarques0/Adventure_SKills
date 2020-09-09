using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPun
{

    public Carousel[] carousels;
    public Button readyButton;
    public Text readyButtonText;
    public Text roomId;

    [HideInInspector]
    public static LobbyManager instance;

    GameObject[] players;

    bool isChoosingClass = true;
    bool readyCountDown;
    Carousel myCarousel;



    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;

        if (roomId != null)
            roomId.text = "ROOM ID: " + PhotonNetwork.CurrentRoom.Name;

        readyButton.onClick.AddListener(ChooseClass);
        if (PhotonNetwork.IsMasterClient)
        {
            AssingCarousel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isChoosingClass && myCarousel != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ChangeCarouselIndex(-1);
            if (Input.GetKeyDown(KeyCode.E))
                ChangeCarouselIndex(1);

            if(Input.GetKeyDown(KeyCode.Return))
            {
                ChooseClass();
            }
        }
        else if(GameManager.singleton.currentPlayer != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (!myCarousel.ready)
                {
                    isChoosingClass = true;
                    GameManager.singleton.DeleteClass();

                    readyButton.onClick.RemoveAllListeners();
                    readyButton.onClick.AddListener(ChooseClass);
                }
                else
                    CallReadyUp();
            }
            if (Input.GetKeyDown(KeyCode.Return))
                CallReadyUp();
        }

        if (CheckIfAllReady() && !readyCountDown)
        {
            readyCountDown = true;
            StartCoroutine(ReadyCountDown());
        }
        else if(!CheckIfAllReady() && readyCountDown)
        {
            readyCountDown = false;
            StopAllCoroutines();
        }

        CheckActivePlayers();
    }

    void ChooseClass()
    {
        isChoosingClass = false;
        GameManager.singleton.ChangeClass(players[myCarousel.targetIndex].name, myCarousel.transform.position);
        GameManager.singleton.playerCharacterIndex = myCarousel.targetIndex;
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(CallReadyUp);
    }

    public void AssingCarousel()
    {
        if (PhotonNetwork.OfflineMode)
        {
            myCarousel = carousels[0];
            SetActiveCarousel(true, 0);
            players = GameManager.singleton.players;
            return;
        }

        for (int x = 0; x < carousels.Length; x++)
        {
            if (x == RoomManager.singleton.playerIndex)
            {
                myCarousel = carousels[x];
                photonView.RPC("SetActiveCarousel", RpcTarget.AllBuffered, true, x);
            }
            else
            {
                foreach (Button btn in carousels[x].arrowsBtns)
                {
                    btn.interactable = false;
                }
            }
        }
        players = GameManager.singleton.players;
    }

    [PunRPC]
    private void SetActiveCarousel(bool value, int index)
    {
        carousels[index].transform.parent.gameObject.SetActive(value);
    }

    void ChangeCarouselIndex(int num)
    {
        myCarousel.CallChangeColumn(num);
    }

    bool CheckIfAllReady()
    {
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (!carousels[i].ready)
                return false;
        }
        return true;
    }

    public void CallReadyUp()
    {
        for(int x = 0; x < carousels.Length; x++)
        {
            if (myCarousel == carousels[x])
            {
                photonView.RPC("ReadyUp", RpcTarget.AllBuffered, x);
                return;
            }
        }
        
    }

    [PunRPC]
    public void ReadyUp(int index)
    {
        Carousel carousel = carousels[index];

        carousel.ready = !carousel.ready;

        if (myCarousel.ready)
            readyButtonText.text = "CANCEL";
        else
            readyButtonText.text = "GO";
    }

    IEnumerator ReadyCountDown()
    {
        for (int x = 3; x >= 0; x--)
        {
            readyButtonText.text = x.ToString();
            yield return new WaitForSeconds(1);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Level");
        }
    }

    private void CheckActivePlayers()
    {
        if (PhotonNetwork.OfflineMode)
            return;

        for(int x = 0; x < PhotonNetwork.CurrentRoom.MaxPlayers; x++)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties["Player" + x] == null || !RoomManager.singleton.PlayerIsOnRoom((string)PhotonNetwork.CurrentRoom.CustomProperties["Player" + x]))
                carousels[x].transform.parent.gameObject.SetActive(false);
            else
                carousels[x].transform.parent.gameObject.SetActive(true);
        }
    }
}
