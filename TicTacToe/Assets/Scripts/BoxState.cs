using System;
using UnityEngine;

public class BoxState : MonoBehaviour
{
    Animator animator;
    GameObject gameGrid;
    GridState gs;
    int gridX;
    int gridY;
    [HideInInspector] public Letter CurrentLetter { get; private set; }

    void Start()
    {
        CurrentLetter = Letter.Blank;
        gridX = (int)Math.Round(this.transform.position.x / 2.0) + 1;
        gridY = (int)Math.Round(this.transform.position.y / 2.0) + 1;
        animator = this.GetComponent<Animator>();
        gameGrid = this.transform.parent.gameObject;
        gs = gameGrid.GetComponent<GridState>();
    }

    public void SetLetter(Letter letter)
    {
        CurrentLetter = letter;
        animator.SetInteger("Letter", (int)CurrentLetter);
        gs.UpdateGridState(gridX, gridY, CurrentLetter);
    }
}
