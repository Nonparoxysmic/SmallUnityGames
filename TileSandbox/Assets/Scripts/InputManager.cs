using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] int diagonalLockFrames;

    GameMaster gm;

    bool mouseMoved;
    int diagonalLockCountdown;
    int lockedDirection;
    int? mouseDirection;
    int previousRawInputDirection;
    Vector3 previousMousePosition;

    void Start()
    {
        gm = GetComponent<GameMaster>();
        if (gm is null)
        {
            this.Error("Missing or unavailable Game Master.");
            return;
        }
        if (cursor is null)
        {
            this.Error("Cursor GameObject not set in Inspector.");
            return;
        }
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (Input.mousePosition != previousMousePosition)
        {
            previousMousePosition = Input.mousePosition;
            mouseMoved = true;
        }
        for (int i = 0; i < Math.Min(gm.ToolbarSize, 9); i++)
        {
            int key = i + 49;
            if (Input.GetKeyDown((KeyCode)key))
            {
                gm.ChangeTool(i);
                break;
            }
        }

        bool shiftIsPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        gm.SetPlayerStrafing(shiftIsPressed);
    }

    void FixedUpdate()
    {
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        int rawInputDirection = Utilities.Direction(vert, horz);

        if (StartOfDiagonalLock(rawInputDirection, previousRawInputDirection))
        {
            diagonalLockCountdown = diagonalLockFrames;
            lockedDirection = previousRawInputDirection;
        }

        int inputDirection;
        if (diagonalLockCountdown > 0)
        {
            if (ContinuingDiagonalLock(rawInputDirection, lockedDirection))
            {
                inputDirection = lockedDirection;
            }
            else
            {
                inputDirection = rawInputDirection;
                diagonalLockCountdown = 0;
            }
        }
        else
        {
            inputDirection = rawInputDirection;
        }
        if (diagonalLockCountdown > 0) { diagonalLockCountdown--; }
        previousRawInputDirection = rawInputDirection;

        gm.SetPlayerFacingDirection(inputDirection);
        gm.OnDirectionalInput(inputDirection);
        UpdateCursorPosition();

        bool actionKeyPressed = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q)
            || Input.GetMouseButton(0);
        gm.OnActionKey(actionKeyPressed, cursor.transform.position);

        mouseMoved = false;
        gm.playerIsMoving = false;
    }

    void UpdateCursorPosition()
    {
        if (mouseMoved || Input.GetMouseButton(0))
        {
            float horz = Input.mousePosition.x - Screen.width / 2;
            float vert = Input.mousePosition.y - Screen.height / 2;
            mouseDirection = Utilities.Direction(vert, horz);
            SetCursorPosition(gm.PlayerFacingPosition((int)mouseDirection));
        }
        else if (gm.playerIsMoving)
        {
            if (mouseDirection is null)
            {
                SetCursorPosition(gm.PlayerFacingPosition());
            }
            else
            {
                SetCursorPosition(gm.PlayerFacingPosition((int)mouseDirection));
            }
        }
        if (!gm.GetPlayerStrafing())
        {
            mouseDirection = null;
        }
    }

    void SetCursorPosition(Vector3 cursorPosition)
    {
        cursorPosition.x = Mathf.Floor(cursorPosition.x + 0.5f);
        cursorPosition.y = Mathf.Floor(cursorPosition.y + 0.5f);
        cursorPosition.z = cursor.transform.position.z;
        cursor.transform.position = cursorPosition;
    }

    bool StartOfDiagonalLock(int direction, int previousDirection)
    {
        return direction.EqualsOneOf(0, 2, 4, 6)
            && Utilities.DirectionsAreAdjacent(direction, previousDirection);
    }

    bool ContinuingDiagonalLock(int direction, int lockedDirection)
    {
        return direction == lockedDirection
            || Utilities.DirectionsAreAdjacent(direction, lockedDirection);
    }
}
