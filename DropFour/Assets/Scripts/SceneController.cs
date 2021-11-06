using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] string mainMenuSceneName;
    [SerializeField] int settingsSceneNumber;
    [SerializeField] GameObject exitButtonTextObject;

    GameMaster gm;
    TMP_Text exitButtonText;

    void Start()
    {
        exitButtonText = exitButtonTextObject.GetComponent<TMP_Text>();
        gm = GameObject.Find("Main Game").GetComponent<GameMaster>();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ChangeExitButtonText(string label)
    {
        exitButtonText.text = label;
    }

    public void OpenSettings()
    {
        gm.isPaused = true;
        SceneManager.LoadSceneAsync(settingsSceneNumber, LoadSceneMode.Additive);
    }
}
