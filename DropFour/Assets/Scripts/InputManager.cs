using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector3 lastMousePosition;
    float lastHorzKey;
    float lastVertKey;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        lastHorzKey = Input.GetAxisRaw("Horizontal");
        lastVertKey = Input.GetAxisRaw("Vertical");
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != lastHorzKey)
        {
            // Horizontal input changed
            
            lastHorzKey = Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetAxisRaw("Vertical") != lastVertKey)
        {
            // Vertical input changed

            lastVertKey = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetMouseButtonDown(0))
        {
            // Left click
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Right click
        }
        if (Input.mousePosition != lastMousePosition)
        {
            // Mouse moved this frame

            lastMousePosition = Input.mousePosition;
        }
    }
}
