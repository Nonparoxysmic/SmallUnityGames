using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] string menuSceneName;

    void Start()
    {
        if (!Utilities.SceneNameIsValid(menuSceneName))
        {
            this.Error($"Invalid scene name \"{menuSceneName}\".");
            return;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
