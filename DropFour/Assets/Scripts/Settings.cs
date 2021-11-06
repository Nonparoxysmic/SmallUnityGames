using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] int settingsSceneNumber;

    GameMaster gm;
    GameObject gmObject;

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
}
