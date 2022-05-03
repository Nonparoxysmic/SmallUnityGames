using System;
using UnityEngine;
using UnityEngine.UI;

public class FitRawImageToCanvas : MonoBehaviour
{
    float previousHeight;
    float previousWidth;
    Rect canvasRect;
    RectTransform canvasTransform;
    RectTransform rawImageTransform;

    void Start()
    {
        if (GetComponent<RawImage>() == null)
        {
            LogErrorAndDisableComponent("GameObject is not a Raw Image.");
            return;
        }
        if (transform.parent == null)
        {
            LogErrorAndDisableComponent("No parent GameObject.");
            return;
        }
        if (transform.parent.GetComponent<Canvas>() == null)
        {
            LogErrorAndDisableComponent("Parent GameObject is not a Canvas.");
            return;
        }

        canvasTransform = transform.parent.GetComponent<RectTransform>();
        rawImageTransform = GetComponent<RectTransform>();

        if (rawImageTransform == null)
        {
            LogErrorAndDisableComponent("Missing or unavailable RectTransform.");
            return;
        }
        if (canvasTransform == null)
        {
            LogErrorAndDisableComponent("Missing or unavailable parent RectTransform.");
            return;
        }
    }

    void LogErrorAndDisableComponent(string message)
    {
        Debug.LogError(name + ": " + nameof(FitRawImageToCanvas) + ": " + message);
        enabled = false;
    }

    void Update()
    {
        canvasRect = canvasTransform.rect;
        if (canvasRect.height != previousHeight || canvasRect.width != previousWidth)
        {
            float imageSize = Math.Min(canvasRect.width, canvasRect.height);
            rawImageTransform.sizeDelta = new Vector2(imageSize, imageSize);
            previousHeight = canvasRect.height;
            previousWidth = canvasRect.width;
        }
    }
}
