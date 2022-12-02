using UnityEngine;

public class SceneInitialization : MonoBehaviour
{
    [SerializeField] GameObject DontDestroyPrefab;
    [SerializeField] GameObject TileLibraryPrefab;

    void Awake()
    {
        if (DontDestroyPrefab == null || TileLibraryPrefab == null)
        {
            this.Error("Prefab reference not set in the inspector.");
            return;
        }
        // Instantiate the DontDestroy prefab if there are
        // no GameObjects with the "DontDestroy" tag.
        GameObject[] dontDestroyObjects = GameObject.FindGameObjectsWithTag("DontDestroy");
        if (dontDestroyObjects.Length > 1)
        {
            this.Error("Multiple \"DontDestroy\" objects found.");
            return;
        }
        else if (dontDestroyObjects.Length == 0)
        {
            // Instantiate the core DontDestroy object.
            GameObject dontDestroy = Instantiate(DontDestroyPrefab);
            dontDestroy.name = DontDestroyPrefab.name;
            // Set DontDestroyOnLoad.
            DontDestroyOnLoad(dontDestroy);
            // Instantiate the child TileLibrary object.
            GameObject tileLibrary = Instantiate(TileLibraryPrefab);
            tileLibrary.name = TileLibraryPrefab.name;
            tileLibrary.transform.parent = dontDestroy.transform;
        }
        Destroy(gameObject);
    }
}
