using UnityEngine;

public class GridState : MonoBehaviour
{
    Letter[,] gridLetters;
    public GameObject testLine;

    void Start()
    {
        gridLetters = new Letter[3, 3];
    }

    public void UpdateGridState(int boxX, int boxY, Letter newLetter)
    {
        gridLetters[boxX, boxY] = newLetter;

        if ((gridLetters[0, 0] != Letter.Blank) && (gridLetters[0, 0] == gridLetters[1, 0]) && (gridLetters[0, 0] == gridLetters[2, 0]))
        {
            testLine.SetActive(true);
        }
        else
        {
            testLine.SetActive(false);
        }
    }    
}
