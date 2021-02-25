using System;
using UnityEngine;

public class MainBoardScript : MonoBehaviour
{
    [SerializeField] GameObject boxGroupPrefab;
    GameObject currentBoxGroup;

    void Start()
    {
        if (boxGroupPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxGroupPrefab = new GameObject();
        }
    }

    public void NewBoxGroup()
    {
        if (currentBoxGroup != null)
        {
            Destroy(currentBoxGroup);
        }
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        currentBoxGroup = Instantiate(boxGroupPrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
        currentBoxGroup.name = "Box Group";
    }

    public void DestroyGame()
    {
        if (currentBoxGroup != null) Destroy(currentBoxGroup);
        GameObject previousLine = GameObject.Find("Line");
        if (previousLine != null) Destroy(previousLine);
    }
}
