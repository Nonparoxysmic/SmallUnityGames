using System;
using UnityEngine;

public class TestGM : MonoBehaviour
{
    Letter playerLetter;
    Letter computerLetter;
    Letter[] letterGrid;
    MainBoardScript mbs;

    void Start()
    {
        mbs = GameObject.Find("Main Board").GetComponent<MainBoardScript>();
        NewGame();
    }

    void NewGame()
    {
        playerLetter = (Letter)UnityEngine.Random.Range(1, 3);
        computerLetter = (Letter)((int)playerLetter % 2 + 1);
        letterGrid = new Letter[9];
        Debug.Log("Player letter is " + playerLetter + ", Computer letter is " + computerLetter);
        mbs.NewBoxGroup();
    }

    void OnMouseDown()
    {
        NewGame();
    }

    public void OnBoxClicked(int boxNumber)
    {
        if (letterGrid[boxNumber] != Letter.Blank) return;
        SetBoxLetter(boxNumber, playerLetter);
        int computerMove = ComputerMoveBox();
        if ((computerMove < 0) || (computerMove > 8)) return;
        SetBoxLetter(computerMove, computerLetter);
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        letterGrid[boxNumber] = newLetter;
        mbs.SetBoxLetter(boxNumber, newLetter);
    }

    int ComputerMoveBox()
    {
        int numOfBlank = 0;
        for (int i = 0; i < 9; i++)
        {
            if (letterGrid[i] == Letter.Blank)
            {
                numOfBlank++;
            }
        }
        if (numOfBlank == 0)
        {
            return -1;
        }
        int tryBox;
        do
        {
            tryBox = UnityEngine.Random.Range(0, 9);
        } while (letterGrid[tryBox] != Letter.Blank);
        return tryBox;
    }
}
