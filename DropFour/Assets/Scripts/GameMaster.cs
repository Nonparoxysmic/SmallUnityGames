using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float tokenAcceleration;
    public float tokenMaxSpeed;

    Engine computerA;
    GameBoard board;
    InputManager inputManager;

    GameResult gameResult;
    GameState currentState;
    GameType gameType;
    int movesMade;

    void Awake()
    {
        board = new GameBoard();
        inputManager = GetComponent<InputManager>();
        if (!PlayerPrefs.HasKey("GameType"))
        {
            PlayerPrefs.SetInt("GameType", (int)GameType.RandomFirst);
        }
        gameType = (GameType)PlayerPrefs.GetInt("GameType");
    }

    void Start()
    {
        computerA = GameObject.Find("Computer Player A").GetComponent<Engine>();
        gameResult = GameResult.InProgress;
        switch (gameType)
        {
            case GameType.RandomFirst:
            case GameType.TwoPlayer:  // TODO: Implement game type
            case GameType.TwoComputer:  // TODO: Implement game type
                currentState = UnityEngine.Random.Range(0, 2) == 0 ? GameState.PlayerTurn : GameState.ComputerTurn;
                break;
            case GameType.PlayerFirst:
                currentState = GameState.PlayerTurn;
                break;
            case GameType.ComputerFirst:
                currentState = GameState.ComputerTurn;
                break;
        }
        if (currentState == GameState.ComputerTurn)
        {
            StartCoroutine(ComputerTurn(0));
        }
        else ShowSelection(true);
    }

    void ShowSelection(bool doShow)
    {
        inputManager.ShowSelection(doShow);
    }

    public void SelectionActivated(int currentSelection)
    {
        if (currentState != GameState.PlayerTurn) return;
        if (currentSelection >= 0 && currentSelection <= 6)
        {
            if (board.IsValidMove(currentSelection))
            {
                ShowSelection(false);
                currentState = GameState.ComputerTurn;
                board.MakeMove(currentSelection);
                inputManager.selectionActivated.Invoke(currentSelection, movesMade & 1);
                if (board.HasConnectedFour(movesMade & 1))
                {
                    currentState = GameState.Ending;
                    gameResult = GameResult.PlayerWin;
                }
                else if (board.MovesMade >= 42)
                {
                    currentState = GameState.Ending;
                    gameResult = GameResult.Tie;
                }
                movesMade++;
                if (currentState == GameState.Ending)
                {
                    currentState = GameState.End;
                    GameOver();
                }
                else
                {
                    StartCoroutine(ComputerTurn(1));
                }
            }
        }
    }

    IEnumerator ComputerTurn(float delaySeconds)
    {
        computerA.StartThinking(board);
        float computerThinkTime = computerA.thinkTime;
        yield return new WaitForSeconds(Math.Max(computerThinkTime + 0.1f, delaySeconds));
        int chosenMove = computerA.Output;
        Debug.Log("Chosen move: " + chosenMove + ", Depth completed: " + computerA.Depth);
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        yield return new WaitForSeconds(0.5f);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
            gameResult = GameResult.ComputerWin;
        }
        else if (board.MovesMade >= 42)
        {
            currentState = GameState.Ending;
            gameResult = GameResult.Tie;
        }
        movesMade++;
        if (currentState == GameState.Ending)
        {
            currentState = GameState.End;
            GameOver();
        }
        else
        {
            currentState = GameState.PlayerTurn;
            ShowSelection(true);
        }
    }

    void GameOver()
    {
        switch (gameResult)
        {
            case GameResult.PlayerWin:
                Debug.Log("PLAYER WINS");
                break;
            case GameResult.ComputerWin:
                Debug.Log("COMPUTER WINS");
                break;
            case GameResult.Tie:
                Debug.Log("TIED GAME");
                break;
        }
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    ComputerTurn,
    Ending,
    End
}

public enum GameResult
{
    InProgress,
    PlayerWin,
    ComputerWin,
    Tie
}
