using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatLabel : MonoBehaviour
{
    public GameDifficulty difficulty;
    public int index;
    GameMasterScript gm;
    Text label;

    void OnEnable()
    {
        if (gm == null)
        {
            gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        }
        if (label == null)
        {
            label = GetComponent<Text>();
        }
        label.text = gm.stats.GetStat(difficulty, index).ToString();
        if (label.text == "0") label.text = "-";
    }
}
