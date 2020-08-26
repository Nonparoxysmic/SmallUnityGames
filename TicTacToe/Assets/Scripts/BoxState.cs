using UnityEngine;

public class BoxState : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public Letter CurrentLetter { get; private set; }

    void Start()
    {
        CurrentLetter = Letter.Blank;
    }

    public void SetLetter(Letter letter)
    {
        CurrentLetter = letter;
        animator.SetInteger("Letter", (int)CurrentLetter);
    }
}
