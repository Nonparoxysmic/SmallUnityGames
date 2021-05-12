using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEngine.Object gameScene;
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
        SceneManager.LoadScene(gameScene.name);
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
