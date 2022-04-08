using System;
using UnityEngine;

public class FitRawImageToCanvas : MonoBehaviour
{
    float previousHeight;
    float previousWidth;
    Rect canvasRect;
    RectTransform canvasTransform;
    RectTransform rawImageTransform;

    void Start()
    {
        canvasTransform = transform.parent.GetComponent<RectTransform>();
        rawImageTransform = GetComponent<RectTransform>();
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
