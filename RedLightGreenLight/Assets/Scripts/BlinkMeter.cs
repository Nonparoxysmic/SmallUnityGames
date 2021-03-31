using System;
using UnityEngine;

public class BlinkMeter : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        sr.color = color;
    }

    public void SetHorzScale(float horzScale)
    {
        transform.localScale = new Vector3(horzScale, transform.localScale.y, transform.localScale.z);
    }
}
