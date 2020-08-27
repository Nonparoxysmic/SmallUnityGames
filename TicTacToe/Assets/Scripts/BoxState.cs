using UnityEngine;

public class BoxState : MonoBehaviour
{
    public Animator animator;
    public GameObject gameGrid;
    GridState gs;
    [HideInInspector] public Letter CurrentLetter { get; private set; }

    void Start()
    {
        CurrentLetter = Letter.Blank;
        gs = gameGrid.GetComponent<GridState>();
    }

    public void SetLetter(Letter letter)
    {
        CurrentLetter = letter;
        animator.SetInteger("Letter", (int)CurrentLetter);
        gs.UpdateGridState();
    }
}
