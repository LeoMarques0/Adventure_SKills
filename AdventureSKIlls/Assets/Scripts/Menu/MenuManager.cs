using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Private Fields

    private Launcher launcher;

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

}
