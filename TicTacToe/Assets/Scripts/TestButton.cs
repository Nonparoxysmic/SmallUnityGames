using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    GameObject gsObj;
    GridState gs;

    public GameObject currentLetterSelection;
    SpriteToggler st;

    void Start()
    {
        gsObj = GameObject.Find("GameGrid");
        gs = gsObj.GetComponent<GridState>();

        currentLetterSelection = GameObject.Find("CurrentLetterSelection");
        st = currentLetterSelection.GetComponent<SpriteToggler>();
    }

    void OnMouseDown()
    {
        if (st.lineHasBeenDrawn || (st.numberOfLetters >= 9))
        {
            return;
        }

        Letter[,] gridLetters = gs.GridLetters;
        Letter selectedLetter;
        int gridX, gridY;
        do {
            gridX = Random.Range(0, 3);
            gridY = Random.Range(0, 3);
            selectedLetter = gridLetters[gridX, gridY];
        } while (selectedLetter != Letter.Blank);

        Debug.Log("Place letter at x:" + gridX + ", y:" + gridY);
    }
}
