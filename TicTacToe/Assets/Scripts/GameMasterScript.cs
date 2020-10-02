using System;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public BoxClickedEvent boxClicked;
    public BoxUpdatedEvent boxUpdated;
    int numberOfMoves;
    Letter playerLetter;
    Letter computerLetter;
    Letter[] letterGrid;
    MainBoardScript mbs;
    
    void Start()
    {
        mbs = GameObject.Find("Main Board").GetComponent<MainBoardScript>();

        if (boxClicked == null) boxClicked = new BoxClickedEvent();
        boxClicked.AddListener(OnBoxClicked);
        if (boxUpdated == null) boxUpdated = new BoxUpdatedEvent();

        NewGame();
    }

    void NewGame()
    {
        playerLetter = (Letter)UnityEngine.Random.Range(1, 3);
        computerLetter = (Letter)((int)playerLetter % 2 + 1);
        numberOfMoves = 0;
        letterGrid = new Letter[9];
        mbs.NewBoxGroup();
    }

    void OnMouseDown()
    {
        NewGame();
    }

    void OnBoxClicked(int boxNumber)
    {
        if (letterGrid[boxNumber] != Letter.Blank) return;
        SetBoxLetter(boxNumber, playerLetter);
        int computerMove = ComputerMoveBox();
        if ((computerMove < 0) || (computerMove > 8)) return;
        SetBoxLetter(computerMove, computerLetter);
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        numberOfMoves++;
        letterGrid[boxNumber] = newLetter;
        boxUpdated.Invoke(boxNumber, newLetter);
    }

    int ComputerMoveBox()
    {
        //if ((int)gd > 0) return -1; // Temporary until difficulty implemented

        if (numberOfMoves >= 9) return -1;

        int randomBlankBoxNumber;
        do
        {
            randomBlankBoxNumber = UnityEngine.Random.Range(0, 9);
        } while (letterGrid[randomBlankBoxNumber] != Letter.Blank);
        return randomBlankBoxNumber;
    }

    //int FindWinningMove(Letter letterToPlay, Letter[] grid)
    //{


    //    return -1; // Temporary
    //}
}
