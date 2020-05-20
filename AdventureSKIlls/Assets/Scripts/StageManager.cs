using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StageManager : MonoBehaviourPun
{

    public Transform[] playersSpawns;
    public Image[] playersIcon;
    public Text[] playersCoins;

    Player myPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        int classIndex = GameManager.singleton.playerCharacterIndex;
        PhotonNetwork.Instantiate(GameManager.singleton.players[classIndex].name, playersSpawns[classIndex].position, Quaternion.identity);
    }

    private void Start()
    {
        myPlayer = GameManager.singleton.currentPlayer.GetComponent<Player>();

        photonView.RPC("SetUIIcon", RpcTarget.AllBuffered, GameManager.singleton.playerCharacterIndex, GameManager.singleton.playerIndex);
    }

    public void CallUpdateCoin()
    {
        photonView.RPC("UpdateCoin", RpcTarget.AllBuffered, myPlayer.coins, GameManager.singleton.playerIndex);
    }

    [PunRPC]
    public void UpdateCoin(int amount, int index)
    {
        playersCoins[index].text = amount.ToString();
    }

    [PunRPC]
    void SetUIIcon(int characterIndex, int index)
    {
        playersIcon[index].sprite = GameManager.singleton.players[characterIndex].GetComponent<Player>().playerIcon;
    }
}
