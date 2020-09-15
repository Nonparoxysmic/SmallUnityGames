using System;
using UnityEngine;

public class BoardState : MonoBehaviour
{
    int boardWidth;
    int boardHeight;
    int victoryNumber;
    Letter[] boardLetters;

    void Start()
    {
        
    }

    public void InitializeBoardState(int width, int height, int victoryNum)
    {
        boardWidth = width;
        boardHeight = height;
        victoryNumber = victoryNum;
        boardLetters = new Letter[width * height];
    }
}
