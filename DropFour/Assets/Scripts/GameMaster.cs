using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float tokenAcceleration;
    public float tokenMaxSpeed;

    InputManager inputManager;
    GameBoard board;
    GameState currentState;
    int movesMade;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        board = new GameBoard();
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
                    Debug.Log("PLAYER WINS");
                    currentState = GameState.End;
                }
                movesMade++;
                if (currentState != GameState.End)
                {
                    StartCoroutine(ComputerTurn(1));
                }
            }
        }
    }

    IEnumerator ComputerTurn(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        int[] validMoves = board.ValidMoves();
        int chosenMove = UnityEngine.Random.Range(0, validMoves.Length);
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        yield return new WaitForSeconds(0.5f);
        if (board.HasConnectedFour(movesMade & 1))
        {
            Debug.Log("COMPUTER WINS");
            currentState = GameState.End;
        }
        movesMade++;
        if (currentState != GameState.End)
        {
            currentState = GameState.PlayerTurn;
            ShowSelection(true);
        }
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    ComputerTurn,
    End
}
