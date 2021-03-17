using System;
using UnityEngine;

public class SquareViewport : MonoBehaviour
{
    Camera cameraComponent;
    int previousScreenHeight;
    int previousScreenWidth;

    void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    void Update()
    {
        if (Screen.height != previousScreenHeight || Screen.width != previousScreenWidth)
        {
            float newViewportWidth = Screen.height / (float)Screen.width;
            float newViewportPosX = (Screen.width - Screen.height) / (2.0f * Screen.width);
            cameraComponent.rect = new Rect(newViewportPosX, 0, newViewportWidth, 1);
            previousScreenHeight = Screen.height;
            previousScreenWidth = Screen.width;
        }
    }
}
