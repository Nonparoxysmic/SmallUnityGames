using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent onPlayerCaught;

    PlayerVision playerVision;

    //List<GameObject> enemies;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
        //enemies = new List<GameObject>();
    }

    void PlayerCaught()
    {
        Time.timeScale = 0;
        playerVision.SetVision(false);
    }
}
