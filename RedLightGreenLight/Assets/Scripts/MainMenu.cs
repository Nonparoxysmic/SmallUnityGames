﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string gameSceneName;
    AudioPlayer audioPlayer;
    GameObject audioPlayerObject;

    void Awake()
    {
        Time.timeScale = 1;
        audioPlayerObject = GameObject.Find("Audio Player");
        if (audioPlayerObject != null)
        {
            audioPlayer = audioPlayerObject.GetComponent<AudioPlayer>();
            audioPlayer.FadeOutMusic();
        }
    }

    public void PlayGame()
    {
        if (audioPlayer != null)
        {
            audioPlayer.FadeInMusic();
        }
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
