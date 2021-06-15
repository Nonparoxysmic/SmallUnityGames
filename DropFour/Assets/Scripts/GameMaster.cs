using System;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    GameBoard board;

    void Start()
    {
        board = new GameBoard();
    }
}
