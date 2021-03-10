using System;
using UnityEngine;
using UnityEngine.Events;

public class GameClock : MonoBehaviour
{
    public UnityEvent onPlayerTick;
    public UnityEvent onNonPlayerTick;
    bool isPlayerTick;

    void Awake()
    {
        onPlayerTick = new UnityEvent();
        onNonPlayerTick = new UnityEvent();
    }

    void FixedUpdate()
    {
        if (isPlayerTick)
        {
            onPlayerTick.Invoke();
            isPlayerTick = false;
        }
        else
        {
            onNonPlayerTick.Invoke();
            isPlayerTick = true;
        }
    }
}
