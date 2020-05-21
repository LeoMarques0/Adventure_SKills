using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Private Fields

    private Launcher launcher;

    [SerializeField]
    private Toggle isPublicToggle;

    #endregion

    #region Public Fields


    public InputField roomIDText;

    public GameObject[] sceneMenus;
    public GameObject menuScreen;
    public GameObject loadScreen;
    public GameObject game;

    #endregion

    private void Start()
    {
        launcher = GetComponent<Launcher>();

        LoadScreen(false);
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
        launcher.CreateRoom();
    }

    public void ChangeMaxPlayers(bool plus)
    {
            if (plus && launcher.maxPlayers < 4)
                launcher.maxPlayers++;
            else if(!plus && launcher.maxPlayers > 2)
                launcher.maxPlayers--;
    }

    public void TurnPublicOnOff()
    {
        launcher.isPublic = isPublicToggle.isOn;
    }

    public void QuickPlay()
    {
        LoadScreen(true);
        launcher.FindRandomRoom();
    }

    public void SearchRoom()
    {
        LoadScreen(true);
        launcher.FindRoom(roomIDText.text);
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

    public void CloseGame()
    {
        Application.Quit();
    }

}
