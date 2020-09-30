using System;
using UnityEngine;

public class MainBoardScript : MonoBehaviour
{
    BoxGroupScript boxGroupScript;
    [SerializeField] GameObject boxGroupPrefab;
    GameObject currentBoxGroup;
    TestGM gm;

    void Start()
    {
        if (boxGroupPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxGroupPrefab = new GameObject();
        }
        gm = GameObject.Find("GameMaster").GetComponent<TestGM>();
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
        boxGroupScript = currentBoxGroup.GetComponent<BoxGroupScript>();
    }

    public void OnBoxClicked(int boxNumber)
    {
        gm.OnBoxClicked(boxNumber);
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        boxGroupScript.SetBoxLetter(boxNumber, newLetter);
    }
}
