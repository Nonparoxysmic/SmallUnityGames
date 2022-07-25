using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string gameSceneName;

    void Start()
    {
        if (!Utilities.SceneNameIsValid(gameSceneName))
        {
            this.Error($"Invalid scene name \"{gameSceneName}\".");
            return;
        }
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
