using System;
using UnityEngine;

public class Column : MonoBehaviour
{
    SpriteRenderer sr;
    Color baseColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void OnMouseDown()
    {
        Debug.Log("Column clicked.");
    }

    void OnMouseEnter()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b) * 0.5f + Color.red * 0.5f;
    }

    void OnMouseExit()
    {
        sr.color = baseColor;
    }
}
