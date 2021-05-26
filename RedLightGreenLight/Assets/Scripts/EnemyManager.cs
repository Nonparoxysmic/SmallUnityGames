using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public string timeElapsed;
    public UnityEvent onPlayerCaught;
    public UnityEvent onPlayerWin;

    [HideInInspector] public bool playerIsCaught;

    AudioSource deathAudio;
    GameMenu gameMenu;
    Text gameOverText;
    MirrorManager mirrorManager;
    PlayerVision playerVision;
    Text progressText;
    Stopwatch stopwatch;

    void Awake()
    {
        onPlayerCaught = new UnityEvent();
        onPlayerCaught.AddListener(PlayerCaught);
        onPlayerWin = new UnityEvent();
        onPlayerWin.AddListener(PlayerWin);
        gameMenu = GameObject.Find("Canvas").GetComponent<GameMenu>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        mirrorManager = GameObject.Find("MirrorManager").GetComponent<MirrorManager>();
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
        progressText = GameObject.Find("ProgressText").GetComponent<Text>();
        stopwatch = new Stopwatch();
        stopwatch.Start();
        deathAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (stopwatch.IsRunning)
        {
            SetProgressText();
        }
    }

    void PlayerCaught()
    {
        playerIsCaught = true;
        Time.timeScale = 0;
        stopwatch.Stop();
        SetProgressText();
        playerVision.SetVision(false);
        gameMenu.StartButtonEnableCountdown();
        deathAudio.Play();
    }

    void PlayerWin()
    {
        gameOverText.text = "YOU WIN";
        stopwatch.Stop();
        SetProgressText();
        playerVision.TerminateBlinking();
        playerVision.SetVision(true);
        gameMenu.StartButtonEnableCountdown();
    }

    void SetProgressText()
    {
        progressText.text = "Mirrors Activated: " + mirrorManager.mirrorsSeen + " / " + mirrorManager.totalMirrors;
        int seconds = (int)Math.Ceiling(stopwatch.Elapsed.TotalSeconds);
        int minutes = seconds / 60;
        seconds -= minutes * 60;
        timeElapsed = seconds < 10 ? minutes + ":0" + seconds : minutes + ":" + seconds;
        progressText.text += Environment.NewLine + "Time: " + timeElapsed;
    }
}
