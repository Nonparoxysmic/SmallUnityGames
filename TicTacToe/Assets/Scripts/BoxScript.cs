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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (newLetter == gm.playerLetter)
        {
            sr.color = new Color(0f, 0f, 1f);
        }
        else sr.color = new Color(1f, 0f, 0f);
        animator.SetInteger("Letter", (int)currentLetter);
    }
}
