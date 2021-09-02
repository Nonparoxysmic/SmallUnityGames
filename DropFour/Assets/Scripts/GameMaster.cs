using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnityEvent_Int : UnityEvent<int> { }
public class UnityEvent_Int_Int : UnityEvent<int, int> { }

public class GameMaster : MonoBehaviour
{
    public UnityEvent_Int selectionChanged;
    public UnityEvent_Int_Int selectionActivated;

    [SerializeField] GameObject[] columns;

    GameBoard board;
    BoxCollider2D[] columnColliders;

    GameState currentState;
    int currentSelection = -1;
    int movesMade;

    void Awake()
    {
        selectionChanged = new UnityEvent_Int();
        selectionActivated = new UnityEvent_Int_Int();
        board = new GameBoard();
        columnColliders = new BoxCollider2D[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            columnColliders[i] = columns[i].GetComponent<BoxCollider2D>();
        }
        currentState = UnityEngine.Random.Range(0, 2) == 0 ? GameState.PlayerTurn : GameState.ComputerTurn;
        if (currentState == GameState.ComputerTurn)
        {
            StartCoroutine(ComputerTurn(0));
        }
    }

    void SelectionChanged(int value)
    {
        currentSelection = value;
        selectionChanged.Invoke(value);
    }

    public void DirectionPressed(Vector2Int direction)
    {
        if (currentSelection < 0)
        {
            if (direction.x > 0) SelectionChanged(0);
            else if (direction.x < 0) SelectionChanged(columns.Length - 1);
        }
        else if (direction.x < 0)
        {
            if (currentSelection > 0)
            {
                SelectionChanged(currentSelection - 1);
            }
            else if (currentSelection == 0)
            {
                SelectionChanged(columns.Length - 1);
            }
        }
        else if (direction.x > 0)
        {
            if (currentSelection < columns.Length - 1)
            {
                SelectionChanged(currentSelection + 1);
            }
            else if (currentSelection == columns.Length - 1)
            {
                SelectionChanged(0);
            }
        }
    }

    public void MouseMoved(Vector2 position)
    {
        for (int i = 0; i < columnColliders.Length; i++)
        {
            if (columnColliders[i].bounds.Contains(position))
            {
                if (i != currentSelection)
                {
                    SelectionChanged(i);
                }
                return;
            }
        }
    }

    public void SelectionActivated()
    {
        if (currentState != GameState.PlayerTurn) return;
        if (currentSelection >= 0 && currentSelection <= 6)
        {
            if (board.IsValidMove(currentSelection))
            {
                currentState = GameState.ComputerTurn;
                board.MakeMove(currentSelection);
                selectionActivated.Invoke(currentSelection, movesMade & 1);
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
        selectionActivated.Invoke(chosenMove, movesMade & 1);
        if (board.HasConnectedFour(movesMade & 1))
        {
            Debug.Log("COMPUTER WINS");
            currentState = GameState.End;
        }
        movesMade++;
        if (currentState != GameState.End)
        {
            currentState = GameState.PlayerTurn;
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
