using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameMasterScript gm;
    GameObject menuPanel;
    GameObject uiPanel;
    GameObject difficultyButtonText;
    GameObject playerLetterText;

    private void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        menuPanel = GameObject.Find("MenuPanel");
        uiPanel = GameObject.Find("uiPanel");
        difficultyButtonText = GameObject.Find("DifficultyButtonText");
        playerLetterText = GameObject.Find("PlayerLetterText");
        uiPanel.SetActive(false);
        difficultyButtonText.GetComponent<Text>().text = "DIFFICULTY: " + gm.difficulty;
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
        GameDifficulty newDifficulty = (GameDifficulty)((int)(gm.difficulty + 1) % 5);
        gm.difficulty = newDifficulty;
        difficultyButtonText.GetComponent<Text>().text = "DIFFICULTY: " + newDifficulty;
    }

    public void UpdatePlayerLetter(Letter newLetter)
    {
        playerLetterText.GetComponent<Text>().text = "Your Letter: " + newLetter;
    }
}
