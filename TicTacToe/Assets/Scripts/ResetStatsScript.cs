using System;
using UnityEngine;
using UnityEngine.UI;

public class ResetStatsScript : MonoBehaviour
{
    readonly string[] textOptions = new string[] { "RESET STATISTICS", "CONFIRM RESET STATISTICS", "STATISTICS RESET" };
    GameMasterScript gm;
    Button button;
    Text buttonText;
    int timesClicked;

    void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        buttonText = GameObject.Find("ResetStatsButtonText").GetComponent<Text>();
        button = buttonText.transform.parent.GetComponent<Button>();
    }

    public void OnClick()
    {
        timesClicked++;
        if (timesClicked == 2)
        {
            gm.stats.ResetStatistics();
            UpdateButtonText();
        }
        else if (timesClicked == 1)
        {
            UpdateButtonText();
        }
    }

    public void ResetTheResetStatsButton()
    {
        timesClicked = 0;
        UpdateButtonText();
        button.interactable = true;
    }

    void UpdateButtonText()
    {
        if (timesClicked == 0)
        {
            buttonText.text = textOptions[0];
        }
        else if (timesClicked == 1)
        {
            buttonText.text = textOptions[1];
            buttonText.color = new Color(0.5f, 0, 0);
            buttonText.fontStyle = FontStyle.Bold;
            return;
        }
        else if (timesClicked > 1)
        {
            buttonText.text = textOptions[2];
            button.interactable = false;
        }
        buttonText.color = new Color(0.08627451f, 0.08627451f, 0.08627451f);
        buttonText.fontStyle = FontStyle.Normal;
    }
}
