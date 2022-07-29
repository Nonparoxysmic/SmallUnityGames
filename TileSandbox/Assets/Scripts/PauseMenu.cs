using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] string menuSceneName;

    void Start()
    {
        if (!Utilities.SceneNameIsValid(menuSceneName))
        {
            this.Error($"Invalid scene name \"{menuSceneName}\".");
            return;
        }
        ShowMenu(false);
    }

    public void ShowMenu(bool doShow)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(doShow);
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
