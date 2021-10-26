using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float computerThinkTime;
    public float tokenAcceleration;
    public float tokenMaxSpeed;

    Engine engine;
    GameBoard board;
    InputManager inputManager;

    GameResult gameResult;
    GameState currentState;
    int movesMade;

    void Awake()
    {
        if (computerThinkTime < 0.1) { computerThinkTime = 0.1f; }
        engine = GetComponent<Engine>();
        board = new GameBoard();
        inputManager = GetComponent<InputManager>();
    }

    void Start()
    {
        gameResult = GameResult.InProgress;
        currentState = UnityEngine.Random.Range(0, 2) == 0 ? GameState.PlayerTurn : GameState.ComputerTurn;
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
        engine.StartThinking(board, computerThinkTime);
        yield return new WaitForSeconds(Math.Max(computerThinkTime + 0.1f, delaySeconds));
        int chosenMove = engine.Output;
        Debug.Log("Chosen move: " + chosenMove + ", Depth completed: " + engine.Depth);
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
