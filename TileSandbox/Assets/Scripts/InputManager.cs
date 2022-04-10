using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector3 inputDirection;

    void FixedUpdate()
    {
        inputDirection.x = (int)Input.GetAxisRaw("Horizontal");
        inputDirection.y = (int)Input.GetAxisRaw("Vertical");
    }
}
