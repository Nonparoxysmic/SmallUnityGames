using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterScript : MonoBehaviour
{
    public BoxClickedEvent boxClicked;
    public BoxUpdatedEvent boxUpdated;
    [SerializeField] GameObject linePrefab;
    GameObject mainBoard;
    MainBoardScript mbs;
    MenuScript menu;
    [HideInInspector] public GameDifficulty difficulty;
    GameState gameState;
    [HideInInspector] public Letter playerLetter;
    Letter computerLetter;
    Letter[] letterGrid;
    int numberOfMoves;
    
    void Start()
    {
        mainBoard = GameObject.Find("Main Board");
        mbs = GameObject.Find("Main Board").GetComponent<MainBoardScript>();
        menu = GetComponent<MenuScript>();
        if (boxClicked == null) boxClicked = new BoxClickedEvent();
        boxClicked.AddListener(OnBoxClicked);
        if (boxUpdated == null) boxUpdated = new BoxUpdatedEvent();
    }

    public void NewGame()
    {
        GameObject previousLine = GameObject.Find("Line");
        if (previousLine != null) Destroy(previousLine);
        playerLetter = (Letter)UnityEngine.Random.Range(1, 3);
        menu.UpdatePlayerLetter(playerLetter);
        computerLetter = (Letter)((int)playerLetter % 2 + 1);
        numberOfMoves = 0;
        letterGrid = new Letter[9];
        mbs.NewBoxGroup();
        gameState = (GameState)UnityEngine.Random.Range(1, 3);
        if (gameState == GameState.CompTurn)
        {
            StartCoroutine(ComputerTurn(0.0f));
        }
    }

    void OnBoxClicked(int boxNumber)
    {
        if (letterGrid[boxNumber] != Letter.Blank) return;
        if (gameState == GameState.PlayerTurn)
        {
            gameState = GameState.CompTurn;

            SetBoxLetter(boxNumber, playerLetter);
            EndStateIfGameOver(playerLetter, letterGrid);

            if (gameState == GameState.CompTurn)
            {
                StartCoroutine(ComputerTurn(1.0f));
            }
        }
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        numberOfMoves++;
        letterGrid[boxNumber] = newLetter;
        boxUpdated.Invoke(boxNumber, newLetter);
    }

    void DrawLine(int lineNum)
    {
        GameObject line = Instantiate(linePrefab, new Vector2(0.0f, 0.0f), Quaternion.identity, mainBoard.transform);
        line.name = "Line";
        float x0, x1, y0, y1;
        float lineRadius = 3.5f;
        switch (lineNum)
        {
            case 0:
            case 1:
            case 2:
                x0 = -lineRadius;
                x1 = lineRadius;
                y0 = 2 - 2 * lineNum;
                y1 = y0;
                break;
            case 3:
            case 4:
            case 5:
                y0 = -lineRadius;
                y1 = lineRadius;
                x0 = 2 * (lineNum - 4);
                x1 = x0;
                break;
            case 6:
                x0 = -lineRadius;
                y0 = lineRadius;
                x1 = lineRadius;
                y1 = -lineRadius;
                break;
            case 7:
                x0 = -lineRadius;
                y0 = -lineRadius;
                x1 = lineRadius;
                y1 = lineRadius;
                break;
            default:
                return;
        }
        float baseX = mainBoard.transform.position.x;
        float baseY = mainBoard.transform.position.y;
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(baseX + x0, baseY + y0, -5);
        positions[1] = new Vector3(baseX + x1, baseY + y1, -5);
        line.GetComponent<LineRenderer>().SetPositions(positions);
    }

    bool FindGameOver(Letter letterPlayed, Letter[] grid)
    {
        for (int line = 0; line < 8; line++)
        {
            CheckLine(letterPlayed, grid, line, out int goodBoxes, out _, out _);
            if (goodBoxes == 3)
            {
                DrawLine(line);
                return true;
            }
        }
        if (numberOfMoves >= 9) return true;
        return false;
    }

    void EndStateIfGameOver(Letter letterPlayed, Letter[] grid)
    {
        if (FindGameOver(letterPlayed, grid))
        {
            gameState = GameState.End;
        }
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

    IEnumerator ComputerTurn(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        int computerBoxChoice = -1;
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                computerBoxChoice = GetRandomMove();
                break;
            case GameDifficulty.Medium:
                if (!FindWinningMove(computerLetter, out computerBoxChoice))
                {
                    computerBoxChoice = GetRandomMove();
                }
                break;
            case GameDifficulty.Hard:
                if (!FindWinningMove(computerLetter, out computerBoxChoice))
                {
                    if (!FindWinningMove(playerLetter, out computerBoxChoice))
                    {
                        computerBoxChoice = GetRandomMove();
                    }
                }
                break;
        }
        SetBoxLetter(computerBoxChoice, computerLetter);
        EndStateIfGameOver(computerLetter, letterGrid);
        if (gameState == GameState.CompTurn) gameState = GameState.PlayerTurn;
    }

    int GetRandomMove()
    {
        int randomBlankBoxNumber;
        do
        {
            randomBlankBoxNumber = UnityEngine.Random.Range(0, 9);
        } while (letterGrid[randomBlankBoxNumber] != Letter.Blank);
        return randomBlankBoxNumber;
    }

    bool FindWinningMove(Letter letterToPlay, out int box)
    {
        for (int line = 0; line < 8; line++)
        {
            CheckLine(letterToPlay, letterGrid, line, out int goodBoxes, out int emptyBoxes, out int theEmptyBox);
            if (goodBoxes == 2 && emptyBoxes == 1)
            {
                box = theEmptyBox;
                return true;
            }
        }
        box = -1;
        return false;
    }
}
