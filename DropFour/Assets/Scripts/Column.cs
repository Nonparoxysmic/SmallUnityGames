using System;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] int selectionValue;

    GameMaster gm;
    SpriteRenderer sr;

    Color baseColor;

    void Awake()
    {
        gm = GameObject.Find("Main Game").GetComponent<GameMaster>();
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void Start()
    {
        gm.selectionChanged.AddListener(OnSelectionChanged);
        gm.selectionActivated.AddListener(OnSelectionActivated);
    }

    void OnSelectionChanged(int value)
    {
        if (value == selectionValue)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b) * 0.5f + Color.red * 0.5f;
        }
        else sr.color = baseColor;
    }

    void OnSelectionActivated(int value)
    {
        if (value == selectionValue)
        {
            Debug.Log("Column " + selectionValue + " activated!");
        }
    }
}
