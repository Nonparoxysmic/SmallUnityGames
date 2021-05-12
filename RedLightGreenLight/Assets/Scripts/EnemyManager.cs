using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent onPlayerCaught;
    public UnityEvent onPlayerWin;

    [HideInInspector] public bool playerIsCaught;

    AudioSource deathAudio;
    GameMenu gameMenu;
    PlayerVision playerVision;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        onPlayerWin = new UnityEvent();
        gameMenu = GameObject.Find("Canvas").GetComponent<GameMenu>();
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
        deathAudio = GetComponent<AudioSource>();
    }

    void PlayerCaught()
    {
        playerIsCaught = true;
        Time.timeScale = 0;
        playerVision.SetVision(false);
        gameMenu.StartButtonEnableCountdown();
        deathAudio.Play();
    }
}
