using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent onPlayerCaught;

    [HideInInspector] public bool playerIsCaught;

    GameMenu gameMenu;
    PlayerVision playerVision;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        gameMenu = GameObject.Find("Canvas").GetComponent<GameMenu>();
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
    }

    void PlayerCaught()
    {
        playerIsCaught = true;
        Time.timeScale = 0;
        playerVision.SetVision(false);
        gameMenu.StartButtonEnableCountdown();
    }
}
