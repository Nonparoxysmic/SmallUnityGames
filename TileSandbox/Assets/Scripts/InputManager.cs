using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static readonly Vector3 tileOffset = new Vector3(0.5f, 0.5f);

    [SerializeField] PlayerController player;
    [SerializeField] Toolbar toolbar;

    [SerializeField] Transform target;

    bool mouseMoved;
    Vector3 previousMousePosition;
    Vector3Int directionalInput;
    Vector3Int mouseDirection;
    Vector3Int targetTile;

    void Start()
    {
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (Input.mousePosition != previousMousePosition)
        {
            mouseMoved = true;
            previousMousePosition = Input.mousePosition;
        }
        for (int i = 1; i <= toolbar.numberOfOptions; i++)
        {
            int key = i + 48;
            if (Input.GetKeyDown((KeyCode)key))
            {
                toolbar.SetOption(i);
                player.ChangeTool(i);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        directionalInput.x = (int)Input.GetAxisRaw("Horizontal");
        directionalInput.y = (int)Input.GetAxisRaw("Vertical");
        int inputDirection = Utilities.Direction(directionalInput.x, directionalInput.y);
        player.isStrafing = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        player.PlayerMovement(inputDirection);

        // Update target cursor position.
        if (mouseMoved || Input.GetMouseButton(0))
        {
            mouseMoved = false;
            mouseDirection.x = (int)Input.mousePosition.x - Screen.width / 2;
            mouseDirection.y = (int)Input.mousePosition.y - Screen.height / 2;
            double angle = Math.Atan2(mouseDirection.y, mouseDirection.x);
            int newDirection = (int)(Math.Round(angle * -4 / Math.PI + 4) + 2) % 8;
            player.targetingVector = player.targetingVectors[newDirection];
        }
        targetTile.x = Mathf.FloorToInt(player.transform.position.x) + player.targetingVector.x;
        targetTile.y = Mathf.FloorToInt(player.transform.position.y) + player.targetingVector.y;
        target.position = targetTile + tileOffset;

        bool testAction = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q)
            || Input.GetMouseButton(0);
        player.TestAction(targetTile, testAction);
    }
}
