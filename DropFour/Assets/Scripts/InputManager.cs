﻿using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameMaster gm;

    Vector3 lastMousePosition;
    float lastHorzKey;
    float lastVertKey;

    void Start()
    {
        gm = gameObject.GetComponent<GameMaster>();
        lastMousePosition = Vector3.positiveInfinity;
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
            gm.MouseMoved(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            lastMousePosition = Input.mousePosition;
        }
    }
}
