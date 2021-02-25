using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterScript : MonoBehaviour
{
    public BoxClickedEvent boxClicked;
    public BoxUpdatedEvent boxUpdated;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject exitButtonText;
    GameObject mainBoard;
    MainBoardScript mbs;
    MenuScript menu;
    [HideInInspector] public GameDifficulty difficulty;
    GameState gameState;
    public Statistics stats;
    [HideInInspector] public Letter playerLetter;
    Letter computerLetter;
    Letter[] letterGrid;
    int numberOfMoves;
    XmlSerializer serializer;
    string saveFilePath;
    
    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/save0.xml";
        Debug.Log("Save file: " + saveFilePath);

        mainBoard = GameObject.Find("Main Board");
        mbs = GameObject.Find("Main Board").GetComponent<MainBoardScript>();
        menu = GetComponent<MenuScript>();
        if (boxClicked == null) boxClicked = new BoxClickedEvent();
        boxClicked.AddListener(OnBoxClicked);
        if (boxUpdated == null) boxUpdated = new BoxUpdatedEvent();
        difficulty = GameDifficulty.Medium;
        stats = new Statistics();
        serializer = new XmlSerializer(typeof(SaveData));
        LoadGame();
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath)) return;
        try
        {
            using (FileStream reader = new FileStream(saveFilePath, FileMode.Open))
            {
                SaveData loadedData = (SaveData)serializer.Deserialize(reader);
                stats.LoadData(loadedData);
            };
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            stats = new Statistics();
        }
    }

    public void SaveGame()
    {
        try
        {
            using (XmlWriter writer = XmlWriter.Create(saveFilePath))
            {
                serializer.Serialize(writer, stats.CreateSaveData());
            };
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
        gameState = stats.GetNextGameState(difficulty);
        stats.gameInProgress = true;
        exitButtonText.GetComponent<Text>().text = "FORFEIT GAME";
        SaveGame();
        if (gameState == GameState.CompTurn)
        {
            StartCoroutine(ComputerTurn(0));
        }
    }

    public void ForfeitGame()
    {
        ResetExitButtonText();
        if (difficulty == GameDifficulty.Easiest)
        {
            stats.AddGame(difficulty, GameResult.Draw);
        }
        else
        {
            stats.AddGame(difficulty, GameResult.Lose);
        }
        SaveGame();
    }

    void ResetExitButtonText()
    {
        exitButtonText.GetComponent<Text>().text = "EXIT TO MENU";
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
                StartCoroutine(ComputerTurn(1));
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

    void OnGameOver(Letter winner)
    {
        ResetExitButtonText();
        if (winner == playerLetter)
        {
            stats.AddGame(difficulty, GameResult.Win);
        }
        else if (winner == computerLetter)
        {
            stats.AddGame(difficulty, GameResult.Lose);
        }
        else stats.AddGame(difficulty, GameResult.Draw);
        SaveGame();
    }

    bool FindGameOver(Letter letterPlayed, Letter[] grid)
    {
        for (int line = 0; line < 8; line++)
        {
            CheckLine(letterPlayed, grid, line, out int goodBoxes, out _, out _);
            if (goodBoxes == 3)
            {
                DrawLine(line);
                OnGameOver(letterPlayed);
                return true;
            }
        }
        if (numberOfMoves >= 9)
        {
            OnGameOver(Letter.Blank);
            return true;
        }
        return false;
    }

    void EndStateIfGameOver(Letter letterPlayed, Letter[] grid)
    {
        if (FindGameOver(letterPlayed, grid))
        {
            gameState = GameState.End;
        }
    }

    public void CheckLine(Letter letterToPlay, Letter[] grid, int lineNum, out int goodBoxes, out int emptyBoxes, out int theEmptyBox)
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
            case GameDifficulty.Easiest:
                if (numberOfMoves == 0) computerBoxChoice = 4;
                else computerBoxChoice = Engine.WorstMove(computerLetter, letterGrid);
                break;
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
            case GameDifficulty.Hardest:
                if (numberOfMoves == 0)
                {
                    int[] corners = new int[] { 0, 2, 6, 8 };
                    computerBoxChoice = corners[UnityEngine.Random.Range(0, 4)];
                }
                else computerBoxChoice = Engine.BestMove(computerLetter, letterGrid);
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
