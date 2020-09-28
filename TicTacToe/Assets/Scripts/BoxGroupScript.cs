using System;
using UnityEngine;

public class BoxGroupScript : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    GameObject[] boxes;

    void Start()
    {
        if (boxPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxPrefab = new GameObject();
        }
        boxes = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            float x = (i % 3 - 1) * 2 + gameObject.transform.position.x;
            float y = ((i - i % 3) / 3 - 1) * -2 + gameObject.transform.position.y;
            boxes[i] = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
            boxes[i].name = "Box " + i;
            boxes[i].GetComponent<BoxScript>().boxNumber = i;
        }
    }
}
