using System;
using UnityEngine;
using UnityEngine.UI;

public class FitRawImageToCanvas : MonoBehaviour
{
    float baseHeight;
    float baseWidth;
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

        rawImageTransform = GetComponent<RectTransform>();
        canvasTransform = transform.parent.GetComponent<RectTransform>();

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

        baseWidth = rawImageTransform.sizeDelta.x;
        baseHeight = rawImageTransform.sizeDelta.y;
    }

    void Update()
    {
        canvasRect = canvasTransform.rect;
        if (canvasRect.width != previousWidth || canvasRect.height != previousHeight)
        {
            previousWidth = canvasRect.width;
            previousHeight = canvasRect.height;

            float scale = Math.Min(canvasRect.width / baseWidth, canvasRect.height / baseHeight);
            rawImageTransform.sizeDelta = new Vector2(scale * baseWidth, scale * baseHeight);
        }
    }

    void LogErrorAndDisableComponent(string message)
    {
        Debug.LogError(name + ": " + nameof(FitRawImageToCanvas) + ": " + message);
        enabled = false;
    }
}
