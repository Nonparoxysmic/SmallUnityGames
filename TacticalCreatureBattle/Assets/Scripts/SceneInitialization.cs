using UnityEngine;

public class SceneInitialization : MonoBehaviour
{
    [SerializeField] GameObject DontDestroyPrefab;

    void Awake()
    {
        // Instantiate the DontDestroyPrefab if there are
        // no GameObjects with the "DontDestroy" tag.
        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroy");
        if (dontDestroyObjects.Length > 1)
        {
            this.Error("Multiple \"DontDestroy\" objects found.");
            return;
        }
        else if (dontDestroyObjects.Length == 0)
        {
            GameObject dontDestroy = Instantiate(DontDestroyPrefab);
            dontDestroy.name = DontDestroyPrefab.name;
            DontDestroyOnLoad(dontDestroy);
        }
        Destroy(gameObject);
    }
}
