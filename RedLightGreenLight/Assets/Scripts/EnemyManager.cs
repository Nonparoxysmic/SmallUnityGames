using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent onPlayerCaught;

    [HideInInspector] public bool playerIsCaught;

    PlayerVision playerVision;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
    }

    void PlayerCaught()
    {
        playerIsCaught = true;
        Time.timeScale = 0;
        playerVision.SetVision(false);
    }
}
