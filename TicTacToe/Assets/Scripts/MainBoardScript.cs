using System;
using UnityEngine;

public class MainBoardScript : MonoBehaviour
{
    BoxGroupScript boxGroupScript;
    [SerializeField] GameObject boxGroupPrefab;
    GameObject currentBoxGroup;

    void Start()
    {
        if (boxGroupPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxGroupPrefab = new GameObject();
        }
        NewBoxGroup();
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
        // Temporary behavior for testing
        if (boxGroupScript.GetBoxLetter(boxNumber) == Letter.X)
        {
            boxGroupScript.SetBoxLetter(boxNumber, Letter.O);
        }
        else
        {
            boxGroupScript.SetBoxLetter(boxNumber, Letter.X);
        }
    }
}
