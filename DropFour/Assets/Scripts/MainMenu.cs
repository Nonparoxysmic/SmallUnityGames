using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string gameSceneName;
    [SerializeField] int settingsSceneNumber;

    TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GameObject.Find("First Player Dropdown").GetComponent<TMP_Dropdown>();
        PlayerPrefs.SetInt("GameType", (int)GameType.RandomFirst);
        if (!PlayerPrefs.HasKey("EngineOneStrength"))
        {
            PlayerPrefs.SetInt("EngineOneStrength", 1);
        }
        if (!PlayerPrefs.HasKey("EngineTwoStrength"))
        {
            PlayerPrefs.SetInt("EngineTwoStrength", 1);
        }
        PlayerPrefs.SetInt("ShowDebugLog", 0);
    }

    public void OpenSettings()
    {
        SceneManager.LoadSceneAsync(settingsSceneNumber, LoadSceneMode.Additive);
    }

    public void UpdateDropdown()
    {
        if (0 <= dropdown.value && dropdown.value <= 2)
        {
            PlayerPrefs.SetInt("GameType", dropdown.value);
        }
    }

    public void TwoPlayer()
    {
        PlayerPrefs.SetInt("GameType", (int)GameType.TwoPlayer);
        StartGame();
    }

    public void ZeroPlayer()
    {
        PlayerPrefs.SetInt("GameType", (int)GameType.TwoComputer);
        StartGame();
    }

    public void StartGame()
    {
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

public enum GameType
{
    RandomFirst,
    PlayerFirst,
    ComputerFirst,
    TwoPlayer,
    TwoComputer
}
