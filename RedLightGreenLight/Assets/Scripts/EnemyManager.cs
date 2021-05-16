using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent onPlayerCaught;
    public UnityEvent onPlayerWin;

    [HideInInspector] public bool playerIsCaught;

    AudioSource deathAudio;
    GameMenu gameMenu;
    Text gameOverText;
    PlayerVision playerVision;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        onPlayerWin = new UnityEvent();
        onPlayerWin.AddListener(PlayerWin);
        gameMenu = GameObject.Find("Canvas").GetComponent<GameMenu>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
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

    void PlayerWin()
    {
        gameOverText.text = "YOU WIN";
        playerVision.TerminateBlinking();
        playerVision.SetVision(true);
        gameMenu.StartButtonEnableCountdown();
    }
}
