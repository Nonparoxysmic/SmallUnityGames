using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEvent_Int : UnityEvent<int> { }

public class GameMaster : MonoBehaviour
{
    public UnityEvent_Int selectionChanged;

    [SerializeField] GameObject[] columns;

    GameBoard board;
    BoxCollider2D[] columnColliders;
    int currentSelection = -1;

    void Awake()
    {
        board = new GameBoard();
        columnColliders = new BoxCollider2D[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            columnColliders[i] = columns[i].GetComponent<BoxCollider2D>();
        }
        selectionChanged = new UnityEvent_Int();
    }

    void SelectionChanged(int value)
    {
        currentSelection = value;
        selectionChanged.Invoke(value);
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
        if (currentSelection >= 0)
        {
            SelectionChanged(-1);
        }
    }
}
