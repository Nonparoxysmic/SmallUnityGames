using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float WalkSpeed = 2.5f;

    bool isMoving;
    int moveCooldown;
    int deltaX;
    int deltaY;

    void Update()
    {
        if (isMoving)
        {
            float newX = transform.position.x + deltaX * WalkSpeed * Time.deltaTime;
            float newY = transform.position.y + deltaY * WalkSpeed * Time.deltaTime;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (moveCooldown > 0)
        {
            moveCooldown--;
            return;
        }
        isMoving = false;
        deltaX = (int)Input.GetAxisRaw("East") - (int)Input.GetAxisRaw("West");
        deltaY = (int)Input.GetAxisRaw("North") - (int)Input.GetAxisRaw("South");
        if (deltaX != 0 && deltaY != 0)
        {
            return;
        }
        if (deltaX != 0 || deltaY != 0)
        {
            isMoving = true;
            moveCooldown = 3;
        }
    }
}
