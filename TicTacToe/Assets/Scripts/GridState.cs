using System;
using UnityEngine;

public class GridState : MonoBehaviour
{
    public string[,] GameGridLetters { get; private set; }

    void Start()
    {
        GameGridLetters = new string[,] { { "", "", "" }, { "", "", "" }, { "", "", "" } };
    }

    string GetGridLetter(int row, int col)
    {
        if (row < 0 || row > 2 || col < 0 || col > 2)
        {
            throw new ArgumentOutOfRangeException();
        }
        return GameGridLetters[row, col];
    }

    void AddGridLetter(int row, int col, string letter)
    {
        if (row < 0 || row > 2 || col < 0 || col > 2)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (GameGridLetters[row, col] != "")
        {
            Debug.Log("AddGridLetter: Overwriting a letter.");
        }
        if (letter == "")
        {
            Debug.Log("AddGridLetter: No letter to write.");
            return;
        }
        GameGridLetters[row, col] = letter;
    }
}
