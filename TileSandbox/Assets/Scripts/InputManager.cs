using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    bool mouseMoved;
    Vector3 previousMousePosition;

    void Start()
    {
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (Input.mousePosition != previousMousePosition)
        {
            previousMousePosition = Input.mousePosition;
            mouseMoved = true;
        }
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
        }
    }
}
