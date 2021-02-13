using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public BoxClickedEvent boxClicked;
    public BoxUpdatedEvent boxUpdated;
    [SerializeField]
    GameDifficulty difficulty;
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
        if (numberOfMoves >= 9) return;
        if (letterGrid[boxNumber] != Letter.Blank) return;
        SetBoxLetter(boxNumber, playerLetter);
        StopIfGameOver(playerLetter, letterGrid);
        int computerMove = ComputerMoveBox();
        if ((computerMove < 0) || (computerMove > 8)) return;
        SetBoxLetter(computerMove, computerLetter);
        StopIfGameOver(computerLetter, letterGrid);
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        numberOfMoves++;
        letterGrid[boxNumber] = newLetter;
        boxUpdated.Invoke(boxNumber, newLetter);
    }

    int ComputerMoveBox()
    {
        if (numberOfMoves >= 9) return -1;
        if ((int)difficulty == 0) return RandomMove();
        return WinOrRandomMove();
    }

    int RandomMove()
    {
        int randomBlankBoxNumber;
        do {
            randomBlankBoxNumber = UnityEngine.Random.Range(0, 9);
        } while (letterGrid[randomBlankBoxNumber] != Letter.Blank);
        return randomBlankBoxNumber;
    }

    int WinOrRandomMove()
    {
        int winningMove = FindWinningMove(computerLetter, letterGrid);
        if (winningMove < 0) return RandomMove();
        return winningMove;
    }

    bool FindGameOver(Letter letterToPlay, Letter[] grid)
    {
        for (int line = 0; line < 8; line++)
        {
            CheckLine(letterToPlay, grid, line, out int goodBoxes, out _, out _);
            if (goodBoxes == 3)
            {
                Debug.Log("Three in a row in line " + line);
                return true;
            }
        }
        return false;
    }

    void StopIfGameOver(Letter letterToPlay, Letter[] grid)
    {
        if (FindGameOver(letterToPlay, grid))
        {
            numberOfMoves = 99;
        }
    }

    int FindWinningMove(Letter letterToPlay, Letter[] grid)
    {
        for (int line = 0; line < 8; line++)
        {
            if (CheckLineForWinningMove(letterToPlay, grid, line, out int move)) return move;
        }
        return -1;
    }

    bool CheckLineForWinningMove(Letter letterToPlay, Letter[] grid, int lineNum, out int move)
    {
        CheckLine(letterToPlay, grid, lineNum, out int goodBoxes, out int emptyBoxes, out int theEmptyBox);

        if (goodBoxes == 2 && emptyBoxes == 1)
        {
            move = theEmptyBox;
            return true;
        }
        move = -1;
        return false;
    }

    void CheckLine(Letter letterToPlay, Letter[] grid, int lineNum, out int goodBoxes, out int emptyBoxes, out int theEmptyBox)
    {
        List<int> boxes = new List<int>();
        switch (lineNum)
        {
            case 0:
                boxes.Add(0);
                boxes.Add(1);
                boxes.Add(2);
                break;
            case 1:
                boxes.Add(3);
                boxes.Add(4);
                boxes.Add(5);
                break;
            case 2:
                boxes.Add(6);
                boxes.Add(7);
                boxes.Add(8);
                break;
            case 3:
                boxes.Add(0);
                boxes.Add(3);
                boxes.Add(6);
                break;
            case 4:
                boxes.Add(1);
                boxes.Add(4);
                boxes.Add(7);
                break;
            case 5:
                boxes.Add(2);
                boxes.Add(5);
                boxes.Add(8);
                break;
            case 6:
                boxes.Add(0);
                boxes.Add(4);
                boxes.Add(8);
                break;
            case 7:
                boxes.Add(2);
                boxes.Add(4);
                boxes.Add(6);
                break;
            default:
                break;
        }

        int _goodBoxes = 0;
        int _emptyBoxes = 0;
        int _theEmptyBox = -1;
        foreach (int box in boxes)
        {
            if (grid[box] == letterToPlay) _goodBoxes++;
            if (grid[box] == Letter.Blank)
            {
                _emptyBoxes++;
                _theEmptyBox = box;
            }
        }

        goodBoxes = _goodBoxes;
        emptyBoxes = _emptyBoxes;
        theEmptyBox = _theEmptyBox;
    }
}
