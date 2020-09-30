using System;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    Animator animator;
    BoxGroupScript parentScript;
    int boxNumber;
    Letter currentLetter;

    void Start()
    {
        animator = GetComponent<Animator>();
        parentScript = gameObject.transform.parent.GetComponent<BoxGroupScript>();
    }

    public void SetBoxNumber(int num)
    {
        boxNumber = num;
    }

    void OnMouseDown()
    {
        parentScript.OnBoxClicked(boxNumber);
    }

    public void SetLetter(Letter newLetter)
    {
        currentLetter = newLetter;
        animator.SetInteger("Letter", (int)currentLetter);
    }
}
