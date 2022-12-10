using UnityEngine;

public class SceneInitialization : MonoBehaviour
{
    [SerializeField] GameObject DontDestroyPrefab;

    void Awake()
    {
        if (DontDestroyPrefab == null)
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
            // Instantiate the DontDestroy object.
            GameObject dontDestroy = Instantiate(DontDestroyPrefab);
            dontDestroy.name = DontDestroyPrefab.name;
            // Set DontDestroyOnLoad.
            DontDestroyOnLoad(dontDestroy);
            // Create the primary state machine.
            GameObject smgo = new GameObject { name = "StateMachine" };
            smgo.transform.parent = dontDestroy.transform;
            StateMachine stateMachine = smgo.AddComponent<StateMachine>();
            stateMachine.ChangeState<StateInitialization>();
        }
        Destroy(gameObject);
    }
}
