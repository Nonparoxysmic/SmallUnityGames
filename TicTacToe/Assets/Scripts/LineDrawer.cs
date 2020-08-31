using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] GameObject lineBase;
    List<GameObject> lines;
    GridState gs;

    void Start()
    {
        gs = GetComponent<GridState>();
        lines = new List<GameObject>();
    }

    void CreateLine()
    {
        GameObject newLine = Instantiate(lineBase);
        newLine.name = "Line" + lines.Count.ToString();
        

        lines.Add(newLine);
    }
}
