using UnityEngine;

public class BoxState : MonoBehaviour
{
    Animator animator;
    GameObject gameGrid;
    GridState gs;
    [HideInInspector] public int gridX;
    [HideInInspector] public int gridY;
    [HideInInspector] public Letter CurrentLetter { get; private set; }

    void Start()
    {
        CurrentLetter = Letter.Blank;
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
