using System;
using UnityEngine;

public class BoxGroupScript : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;

    void Start()
    {
        if (boxPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxPrefab = new GameObject();
        }
        for (int i = 0; i < 9; i++)
        {
            float x = gameObject.transform.position.x + (i % 3 - 1) * 2;
            float y = gameObject.transform.position.y - ((i - i % 3) / 3 - 1) * 2;
            GameObject newBox = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
            newBox.name = "Box " + i;
            newBox.GetComponent<BoxScript>().SetBoxNumber(i);
        }
    }
}
