using System;
using UnityEngine;

public class FitRawImageToCanvas : MonoBehaviour
{
    RectTransform canvasTransform;
    RectTransform rawImageTransform;

    void Start()
    {
        canvasTransform = transform.parent.GetComponent<RectTransform>();
        rawImageTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float imageSize = Math.Min(canvasTransform.rect.width, canvasTransform.rect.height);
        rawImageTransform.sizeDelta = new Vector2(imageSize, imageSize);
    }
}
