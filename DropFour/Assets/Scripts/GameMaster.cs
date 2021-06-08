using System;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Sprite placeholderToken;
    Color tokenColor = Color.red;

    BoardSpace[,] board;

    void Start()
    {
        board = new BoardSpace[7, 6];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DebugPlayMove((int)Math.Floor(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 0.5) + 3);
        }
    }

    void DebugPlayMove(int move)
    {
        if (move < 0 || move > 6) return;
        if (board[move, 5] != BoardSpace.Empty) return;
        float yPos = 0;
        for (int i = 0; i < 6; i++)
        {
            if (board[move, i] == BoardSpace.Empty)
            {
                board[move, i] = BoardSpace.PlayerOne;
                yPos = i - 2.5f;
                break;
            }
        }
        GameObject debugToken = new GameObject();
        debugToken.name = "Debug Token";
        debugToken.transform.position = new Vector3(debugToken.transform.position.x - 3 + move, debugToken.transform.position.y + yPos, debugToken.transform.position.z);
        debugToken.transform.parent = gameObject.transform;
        SpriteRenderer sr = debugToken.AddComponent<SpriteRenderer>();
        sr.sprite = placeholderToken;
        tokenColor = new Color(tokenColor.r, (tokenColor.g + 1) % 2, tokenColor.b);
        sr.color = tokenColor;
    }
}

public enum BoardSpace
{
    Empty,
    PlayerOne,
    PlayerTwo
}
