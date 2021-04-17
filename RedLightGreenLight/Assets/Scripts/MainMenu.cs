using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEngine.Object gameScene;
    GameObject audioPlayerObject;

    void Awake()
    {
        Time.timeScale = 1;
        audioPlayerObject = GameObject.Find("Audio Player");
        if (audioPlayerObject != null)
        {
            AudioPlayer audioPlayer = audioPlayerObject.GetComponent<AudioPlayer>();
            audioPlayer.PlayMusic();
        }
    }

    public void PlayGame()
    {
        if (audioPlayerObject != null)
        {
            AudioPlayer audioPlayer = audioPlayerObject.GetComponent<AudioPlayer>();
            audioPlayer.FadeOutMusic();
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
