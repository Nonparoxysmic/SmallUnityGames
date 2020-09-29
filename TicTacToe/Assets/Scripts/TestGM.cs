using System;
using UnityEngine;

public class TestGM : MonoBehaviour
{
    GameState gameState;
    MainBoardScript mbs;

    void Start()
    {
        mbs = GameObject.Find("Main Board").GetComponent<MainBoardScript>();
    }

    void OnMouseDown()
    {
        mbs.NewBoxGroup();
    }
}
