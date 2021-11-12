using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugOutput : MonoBehaviour
{
    public TMP_Text output;

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
        output.text = string.Join(Environment.NewLine, lines);
    }
}
