using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] string mainMenuSceneName;
    [SerializeField] GameObject exitButtonTextObject;

    TMP_Text exitButtonText;

    void Start()
    {
        exitButtonText = exitButtonTextObject.GetComponent<TMP_Text>();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ChangeExitButtonText(string label)
    {
        exitButtonText.text = label;
    }
}
