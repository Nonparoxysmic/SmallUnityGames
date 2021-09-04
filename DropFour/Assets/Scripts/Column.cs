﻿using System;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] GameObject tokenPrefab;
    [SerializeField] int selectionValue;

    GameMaster gm;
    SpriteRenderer sr;

    bool showSelection;
    Color baseColor;
    int tokenCount;

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
        gm.showSelectionChanged.AddListener(ShowSelectionChanged);
    }

    void OnSelectionChanged(int value)
    {
        if (value == selectionValue && showSelection)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b) * 0.5f + Color.red * 0.5f;
        }
        else sr.color = baseColor;
    }

    void OnSelectionActivated(int selection, int player)
    {
        if (selection == selectionValue)
        {
            Debug.Log("Column " + selectionValue + " activated!");
            GameObject tokenObject = Instantiate(tokenPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3.5f, -1), Quaternion.identity, gameObject.transform);
            tokenObject.GetComponent<SpriteRenderer>().color = new Color(1, player, 0);
            tokenObject.GetComponent<Token>().SetFallPositionY(-2.5f + tokenCount++);
        }
    }

    void ShowSelectionChanged(bool doShow)
    {
        showSelection = doShow;
    }
}
