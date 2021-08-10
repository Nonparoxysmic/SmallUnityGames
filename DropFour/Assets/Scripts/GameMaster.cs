using System;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] GameObject[] columns;

    GameBoard board;
    BoxCollider2D[] columnColliders;

    void Start()
    {
        board = new GameBoard();
        columnColliders = new BoxCollider2D[columns.Length];
        for (int i = 0; i < columns.Length; i++)
        {
            columnColliders[i] = columns[i].GetComponent<BoxCollider2D>();
        }
    }

    public void MouseMoved(Vector2 position)
    {
        foreach (BoxCollider2D columnCollider in columnColliders)
        {
            
        }
    }
}
