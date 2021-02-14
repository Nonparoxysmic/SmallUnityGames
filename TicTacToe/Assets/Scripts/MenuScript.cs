using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    GameMasterScript gm;
    GameObject menuPanel;
    GameObject uiPanel;
    [SerializeField] GameObject difficultyButtonText;
    [SerializeField] GameObject playerLetterText;

    private void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        menuPanel = GameObject.Find("MenuPanel");
        uiPanel = GameObject.Find("uiPanel");
        uiPanel.SetActive(false);
        difficultyButtonText.GetComponent<Text>().text = "DIFFICULTY: " + gm.difficulty;
    }

    public void ExitToMenu()
    {
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
        GameDifficulty newDifficulty = (GameDifficulty)((int)(gm.difficulty + 1) % 3);
        gm.difficulty = newDifficulty;
        difficultyButtonText.GetComponent<Text>().text = "DIFFICULTY: " + newDifficulty;
    }

    public void UpdatePlayerLetter()
    {
        playerLetterText.GetComponent<Text>().text = "Your Letter: " + gm.playerLetter;
    }
}
