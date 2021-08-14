using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameMaster gm;

    Vector3 lastMousePosition;

    void Awake()
    {
        gm = GetComponent<GameMaster>();
        lastMousePosition = Vector3.positiveInfinity;
    }

    void Update()
    {
        int horizontal = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            horizontal--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            horizontal++;
        }
        if (horizontal != 0)
        {
            gm.DirectionPressed(new Vector2Int(horizontal, 0));
        }

        Vector3 mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            // Left click
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Right click
        }
        if (mousePos != lastMousePosition)
        {
            gm.MouseMoved(Camera.main.ScreenToWorldPoint(mousePos));
            lastMousePosition = mousePos;
        }
    }
}
