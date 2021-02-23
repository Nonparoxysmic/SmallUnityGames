using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameMasterScript gm;
    GameObject menuPanel;
    GameObject uiPanel;
    Dropdown difficultyDropdown;
    GameObject playerLetterText;

    private void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        menuPanel = GameObject.Find("MenuPanel");
        uiPanel = GameObject.Find("uiPanel");
        difficultyDropdown = GameObject.Find("DifficultyDropdown").GetComponent<Dropdown>();
        playerLetterText = GameObject.Find("PlayerLetterText");
        uiPanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        if (gm.stats.gameInProgress)
        {
            gm.ForfeitGame();
        }
        uiPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void NewGame()
    {
        menuPanel.SetActive(false);
        uiPanel.SetActive(true);
        gm.NewGame();
    }

    public void ChangeDifficulty()
    {
        gm.difficulty = (GameDifficulty)difficultyDropdown.value;
    }

    public void UpdatePlayerLetter(Letter newLetter)
    {
        playerLetterText.GetComponent<Text>().text = "Your Letter: " + newLetter;
    }
}
