using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    public GameObject[] players;
    public int playerCharacterIndex;

    public int playerIndex;
    public int coins;

    public GameObject currentPlayer;

    private List<string> upgrades = new List<string>();

    private void Awake()
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

    public void BuyUpgrade(string upgradeBought)
    {
        upgrades.Add(upgradeBought);
    }

    public void ApplyUpgrades()
    {

        Player currentStats = currentPlayer.GetComponent<Player>();

        foreach (string upgrade in upgrades)
        {
            switch(upgrade)
            {
                case "HEALTH":

                    currentStats.maxHealth += 5;
                    currentStats.health += 5;

                    break;

                case "ATTACK":

                    currentStats.additionalDamage += 1;

                    break;

                case "SPEED":

                    currentStats.maxSpd += (currentStats.maxSpd * .1f);

                    break;
            }
        }
    }
}
