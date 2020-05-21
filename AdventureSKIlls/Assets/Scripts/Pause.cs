using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Pause : MonoBehaviour
{

    public Player myPlayer;

    public void Resume()
    {
        myPlayer.isPaused = false;
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
