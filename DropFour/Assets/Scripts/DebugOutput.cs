using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugOutput : MonoBehaviour
{
    public Text debugOutputText;
    public Image debugBackgroundImage;

    Queue<string> lines;

    void Awake()
    {
        lines = new Queue<string>();
    }

    public void AddText(string text)
    {
        Debug.Log(text);
        lines.Enqueue(text);
        if (lines.Count > 4)
        {
            lines.Dequeue();
        }
        debugOutputText.text = string.Join(Environment.NewLine, lines);
    }
}
