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

    GameState currentState;
    int movesMade;

    void Awake()
    {
        if (computerThinkTime < 1) { computerThinkTime = 1; }
        engine = GetComponent<Engine>();
        board = new GameBoard();
        inputManager = GetComponent<InputManager>();
    }

    void Start()
    {
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
                }
                movesMade++;
                if (currentState == GameState.Ending)
                {
                    currentState = GameState.End;
                    GameOver(true);
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
        Debug.Log("Chosen move: " + engine.Output);  // TEMPORARY FOR DEBUGGING
        int chosenMove = engine.RandomMove(board);  // TEMPORARY FOR DEBUGGING
        //int chosenMove = engine.Output;
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        yield return new WaitForSeconds(0.5f);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
        }
        movesMade++;
        if (currentState == GameState.Ending)
        {
            currentState = GameState.End;
            GameOver(false);
        }
        else
        {
            currentState = GameState.PlayerTurn;
            ShowSelection(true);
        }
    }

    void GameOver(bool playerWon)
    {
        if (playerWon)
        {
            Debug.Log("PLAYER WINS");
        }
        else
        {
            Debug.Log("COMPUTER WINS");
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
