using UnityEngine;

public class GridState : MonoBehaviour
{
    [HideInInspector] public Letter[,] GridLetters { get; private set; }

    void Start()
    {
        GridLetters = new Letter[3, 3];
    }

    public void UpdateGridState(int boxX, int boxY, Letter newLetter)
    {
        GridLetters[boxX, boxY] = newLetter;
    }    
}
