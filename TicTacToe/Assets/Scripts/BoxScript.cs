using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxScript : MonoBehaviour
{
    Animator animator;
    GameMasterScript gm;
    int boxNumber;
    Letter currentLetter;

    void Start()
    {
        animator = GetComponent<Animator>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
        gm.boxUpdated.AddListener(UpdateBox);
    }

    public void SetBoxNumber(int num)
    {
        boxNumber = num;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        gm.boxClicked.Invoke(boxNumber);
    }

    void UpdateBox(int newNumber, Letter newLetter)
    {
        if (newNumber == boxNumber)
        {
            SetLetter(newLetter);
        }
    }

    void SetLetter(Letter newLetter)
    {
        currentLetter = newLetter;
        animator.SetInteger("Letter", (int)currentLetter);
    }
}
