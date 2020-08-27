using UnityEngine;

public class GridState : MonoBehaviour
{
    Letter[,] gridLetters;
    public BoxState[] boxStates;
    public GameObject testLine;

    void Start()
    {
        gridLetters = new Letter[3, 3];
        UpdateGridState();
    }

    public void UpdateGridState()
    {
        gridLetters[0, 0] = boxStates[6].CurrentLetter;
        gridLetters[1, 0] = boxStates[7].CurrentLetter;
        gridLetters[2, 0] = boxStates[8].CurrentLetter;
        gridLetters[0, 1] = boxStates[3].CurrentLetter;
        gridLetters[1, 1] = boxStates[4].CurrentLetter;
        gridLetters[2, 1] = boxStates[5].CurrentLetter;
        gridLetters[0, 2] = boxStates[0].CurrentLetter;
        gridLetters[1, 2] = boxStates[1].CurrentLetter;
        gridLetters[2, 2] = boxStates[2].CurrentLetter;

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
