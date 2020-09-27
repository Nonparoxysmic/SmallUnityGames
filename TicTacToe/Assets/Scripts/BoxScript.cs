using System;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    Animator animator;
    [HideInInspector] public int boxNumber;
    Letter currentLetter;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        Debug.Log("Box " + boxNumber + " clicked.");
    }

    public void SetLetter(Letter newLetter)
    {
        currentLetter = newLetter;
    }
}
