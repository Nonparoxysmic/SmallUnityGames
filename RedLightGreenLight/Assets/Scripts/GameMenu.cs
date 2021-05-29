using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] string menuSceneName;
    [SerializeField] Button returnButton;
    [SerializeField] Text buttonText;
    public float buttonDelay;

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void StartButtonEnableCountdown()
    {
        StartCoroutine(EnableButton(buttonDelay));
    }

    IEnumerator EnableButton(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        buttonText.text = "Return To Menu";
        returnButton.interactable = true;
    }
}
