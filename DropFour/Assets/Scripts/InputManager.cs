using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] float repeatDelay;
    [SerializeField] float repeatInterval;

    GameMaster gm;

    bool holdingLeft;
    bool holdingRight;
    float holdingLeftDelay;
    float holdingRightDelay;
    DateTime holdLeftStarted;
    DateTime holdRightStarted;
    Vector3 lastMousePosition;

    void Awake()
    {
        gm = GetComponent<GameMaster>();
        lastMousePosition = Vector3.positiveInfinity;
        repeatInterval = Math.Max(repeatInterval, 0.05f);
    }

    void Update()
    {
        bool pressedLeft = false;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (holdingLeft)
            {
                if ((DateTime.UtcNow - holdLeftStarted).TotalSeconds > repeatInterval + holdingLeftDelay)
                {
                    pressedLeft = true;
                    holdLeftStarted = DateTime.UtcNow;
                    holdingLeftDelay = 0;
                }
            }
            else
            {
                pressedLeft = true;
                holdLeftStarted = DateTime.UtcNow;
                holdingLeftDelay = repeatDelay;
                holdingLeft = true;
            }
        }
        else holdingLeft = false;

        bool pressedRight = false;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (holdingRight)
            {
                if ((DateTime.UtcNow - holdRightStarted).TotalSeconds > repeatInterval + holdingRightDelay)
                {
                    pressedRight = true;
                    holdRightStarted = DateTime.UtcNow;
                    holdingRightDelay = 0;
                }
            }
            else
            {
                pressedRight = true;
                holdRightStarted = DateTime.UtcNow;
                holdingRightDelay = repeatDelay;
                holdingRight = true;
            }
        }
        else holdingRight = false;

        int horizontal = 0;
        if (pressedLeft) horizontal--;
        if (pressedRight) horizontal++;
        if (holdingLeft && holdingRight) horizontal = 0;

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
