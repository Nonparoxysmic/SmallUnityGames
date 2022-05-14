using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;

    GameMaster gm;

    bool mouseMoved;
    int prevDirection = 2;
    Vector3 previousMousePosition;

    void Start()
    {
        gm = GetComponent<GameMaster>();
        if (gm is null)
        {
            Utilities.ComponentError(this, "Missing or unavailable Game Master.");
            return;
        }
        if (cursor is null)
        {
            Utilities.ComponentError(this, "Cursor GameObject not set in Inspector.");
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

        Vector3 pos = gm.PlayerColliderPosition() + Utilities.DirectionVector(prevDirection);
        pos.x = Mathf.Floor(pos.x + 0.5f);
        pos.y = Mathf.Floor(pos.y + 0.5f);
        pos.z = cursor.transform.position.z;
        cursor.transform.position = pos;
    }

    void FixedUpdate()
    {
        int horz = (int)Input.GetAxisRaw("Horizontal");
        int vert = (int)Input.GetAxisRaw("Vertical");
        int inputDirection;
        if (horz == 0 && vert == 0)
        {
            inputDirection = -8;
        }
        else
        {
            inputDirection = (int)(4 * (Math.Atan2(vert, horz) / Math.PI + 1) % 8);
            prevDirection = inputDirection;
        }
        gm.OnDirectionalInput(inputDirection);
    }
}
