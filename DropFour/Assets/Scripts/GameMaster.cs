using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEvent_Int : UnityEvent<int> { }

public class GameMaster : MonoBehaviour
{
    public UnityEvent_Int selectionChanged;
    public UnityEvent_Int selectionActivated;

    [SerializeField] GameObject[] columns;

    GameBoard board;
    BoxCollider2D[] columnColliders;

    int currentSelection = -1;

    void Awake()
    {
        selectionChanged = new UnityEvent_Int();
        selectionActivated = new UnityEvent_Int();
        board = new GameBoard();
        columnColliders = new BoxCollider2D[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            columnColliders[i] = columns[i].GetComponent<BoxCollider2D>();
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
        selectionActivated.Invoke(currentSelection);
    }
}
