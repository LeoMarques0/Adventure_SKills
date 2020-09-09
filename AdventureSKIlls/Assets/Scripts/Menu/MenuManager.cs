using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    #region Private Fields

    [SerializeField]
    private Toggle isPublicToggle;

    [SerializeField]
    private AudioMixer soundMixer = null, musicMixer = null;
    [SerializeField]
    private Text maxPlayersText = null;
    [SerializeField]
    private Text roomIDText = null;
    [SerializeField]
    private string roomToSearch = "";

    #endregion

    #region Public Fields

    public GameObject[] sceneMenus;
    public GameObject menuScreen;
    public GameObject loadScreen;
    public GameObject game;

    #endregion

    private void Start()
    {
        if(loadScreen != null && menuScreen != null)
            LoadScreen(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && maxPlayersText != null)
            maxPlayersText.text = Launcher.singleton.maxPlayers.ToString();
    }

    public void LoadScene(string sceneName)
    {
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeMenu(string desiredMenu)
    {
        foreach(GameObject menu in sceneMenus)
        {
            if (menu.name != desiredMenu)
                menu.SetActive(false);
            else
                menu.SetActive(true);
        }
    }

    public void CreateRoom()
    {
        LoadScreen(true);
        Launcher.singleton.CreateRoom();
    }

    public void ChangeMaxPlayers(bool plus)
    {
            if (plus && Launcher.singleton.maxPlayers < 4)
            Launcher.singleton.maxPlayers++;
            else if(!plus && Launcher.singleton.maxPlayers > 2)
            Launcher.singleton.maxPlayers--;
    }

    public void TurnPublicOnOff()
    {
        Launcher.singleton.isPublic = isPublicToggle.isOn;
    }

    public void QuickPlay()
    {
        LoadScreen(true);
        Launcher.singleton.FindRandomRoom();
    }

    public void CallCreateRoom()
    {
        Launcher.singleton.CreateRoom();
        LoadScreen(true);
    }

    public void CallFindRoom()
    {
        if (roomToSearch != "")
        {
            Launcher.singleton.FindRoom(roomToSearch);
            LoadScreen(true);
        }
    }

    public void CallFindRandomRoom()
    {
        Launcher.singleton.FindRandomRoom();
        LoadScreen(true);
    }

    public void ChangeRoomName(string newValue)
    {
        Launcher.singleton.roomID = newValue;
    }

    public void SearchRoomText(string newValue)
    {
        roomToSearch = newValue;
    }

    public void SearchRoom()
    {
        LoadScreen(true);
        Launcher.singleton.FindRoom(roomIDText.text);
    }

    public void LoadScreen(bool on)
    {
        loadScreen.SetActive(on);
        menuScreen.SetActive(!on);
    }

    public void ExitMenu()
    {
        loadScreen.SetActive(false);
        menuScreen.SetActive(false);

        game.SetActive(true);
    }

    public void ExitLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    public void ChangeSoundVolume(float sliderValue)
    {
        soundMixer.SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}
