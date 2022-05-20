using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] int diagonalLockFrames;

    GameMaster gm;

    bool mouseMoved;
    int diagonalLockCountdown;
    int inputDirection;
    int lockedDirection;
    int previousRawInputDirection;
    Vector3 mouseDirection;
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

        bool shiftIsPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        gm.SetPlayerStrafing(shiftIsPressed);
    }

    void FixedUpdate()
    {
        int horz = (int)Input.GetAxisRaw("Horizontal");
        int vert = (int)Input.GetAxisRaw("Vertical");
        int rawInputDirection;
        if (horz == 0 && vert == 0)
        {
            rawInputDirection = -8;
        }
        else
        {
            rawInputDirection = (int)(4 * (Math.Atan2(vert, horz) / Math.PI + 1) % 8);
        }

        // If raw input direction just changed from a diagonal to an adjacent orthogonal...
        if (rawInputDirection.EqualsOneOf(0, 2, 4, 6)
            && Utilities.DirectionsAreAdjacent(rawInputDirection, previousRawInputDirection))
        {
            // Lock the diagonal orientation for a few frames.
            diagonalLockCountdown = diagonalLockFrames;
            lockedDirection = previousRawInputDirection;
        }
        if (diagonalLockCountdown > 0)
        {
            if (rawInputDirection == lockedDirection
                || Utilities.DirectionsAreAdjacent(rawInputDirection, lockedDirection))
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

        mouseMoved = false;
        gm.playerIsMoving = false;
    }

    void UpdateCursorPosition()
    {
        if (mouseMoved || Input.GetMouseButton(0))
        {
            mouseDirection.x = Input.mousePosition.x - Screen.width / 2;
            mouseDirection.y = Input.mousePosition.y - Screen.height / 2;
            double angle = Math.Atan2(mouseDirection.y, mouseDirection.x);
            int direction = (int)(4 * (angle / Math.PI + 1) % 8);
            SetCursorPosition(gm.PlayerFacingPosition(direction));
        }
        else if (gm.playerIsMoving)
        {
            SetCursorPosition(gm.PlayerFacingPosition());
        }
    }

    void SetCursorPosition(Vector3 cursorPosition)
    {
        cursorPosition.x = Mathf.Floor(cursorPosition.x + 0.5f);
        cursorPosition.y = Mathf.Floor(cursorPosition.y + 0.5f);
        cursorPosition.z = cursor.transform.position.z;
        cursor.transform.position = cursorPosition;
    }
}
