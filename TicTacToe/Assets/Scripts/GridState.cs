using UnityEngine;

public class GridState : MonoBehaviour
{
    [HideInInspector] public Letter[,] GridLetters { get; private set; }
    LineDrawer ld;

    void Start()
    {
        GridLetters = new Letter[3, 3];
        ld = GetComponent<LineDrawer>();
    }

    public void UpdateGridState(int boxX, int boxY, Letter newLetter)
    {
        GridLetters[boxX, boxY] = newLetter;
        ld.DrawLines();
    }    
}
