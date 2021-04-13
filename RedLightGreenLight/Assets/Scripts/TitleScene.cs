using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] UnityEngine.Object mainMenuScene;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuScene.name);
        }
    }
}
