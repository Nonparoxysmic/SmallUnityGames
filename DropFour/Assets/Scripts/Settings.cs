using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] int settingsSceneNumber;
    [SerializeField] Slider engineOneSlider;
    [SerializeField] Slider engineTwoSlider;

    GameMaster gm;
    GameObject gmObject;

    void Awake()
    {
        engineOneSlider.value = PlayerPrefs.GetInt("EngineOneStrength");
        engineTwoSlider.value = PlayerPrefs.GetInt("EngineTwoStrength");
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
}
