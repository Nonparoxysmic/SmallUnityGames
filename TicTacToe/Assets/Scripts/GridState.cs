using UnityEngine;

public class GridState : MonoBehaviour
{
    Letter[,] gridLetters;

    void Start()
    {
        gridLetters = new Letter[3, 3];
    }

    public void UpdateGridState(int boxX, int boxY, Letter newLetter)
    {
        gridLetters[boxX, boxY] = newLetter;
    }    
}
