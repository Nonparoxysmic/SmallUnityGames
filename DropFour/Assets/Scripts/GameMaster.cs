using System;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Sprite placeholderToken;

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
        GameObject debugToken = new GameObject();
        debugToken.name = "Debug Token";
        debugToken.transform.position = new Vector3(debugToken.transform.position.x - 3 + move, debugToken.transform.position.y, debugToken.transform.position.z);
        debugToken.transform.parent = gameObject.transform;
        SpriteRenderer sr = debugToken.AddComponent<SpriteRenderer>();
        sr.sprite = placeholderToken;
    }
}

public enum BoardSpace
{
    Empty,
    PlayerOne,
    PlayerTwo
}
