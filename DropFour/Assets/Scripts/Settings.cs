using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] int settingsSceneNumber;
    [SerializeField] Slider engineOneSlider;
    [SerializeField] Slider engineTwoSlider;
    [SerializeField] Toggle debugTextToggle;

    GameMaster gm;
    GameObject gmObject;

    void Awake()
    {
        engineOneSlider.value = PlayerPrefs.GetInt("EngineOneStrength");
        engineTwoSlider.value = PlayerPrefs.GetInt("EngineTwoStrength");
        debugTextToggle.isOn = PlayerPrefs.GetInt("ShowDebugLog") != 0;
    }

    void Start()
    {
        gmObject = GameObject.Find("Main Game");
        if (gmObject != null)
        {
            gm = gmObject.GetComponent<GameMaster>();
        }
    }

    public void CloseSettings()
    {
        SceneManager.UnloadSceneAsync(settingsSceneNumber);
        if (gm != null)
        {
            gm.isPaused = false;
        }
    }

    public void UpdateEngineOneStrength()
    {
        PlayerPrefs.SetInt("EngineOneStrength", (int)engineOneSlider.value);
    }

    public void UpdateEngineTwoStrength()
    {
        PlayerPrefs.SetInt("EngineTwoStrength", (int)engineTwoSlider.value);
    }

    public void UpdateDebug()
    {
        int showDebug = debugTextToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("ShowDebugLog", showDebug);
        if (gm != null)
        {
            gm.ShowDebugText(debugTextToggle.isOn);
        }
    }
}
