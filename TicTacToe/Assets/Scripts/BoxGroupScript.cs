using System;
using UnityEngine;
using UnityEngine.Experimental.Audio.Google;

public class BoxGroupScript : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    GameObject[] boxes;
    MainBoardScript parentScript;

    void Start()
    {
        if (boxPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            boxPrefab = new GameObject();
        }
        parentScript = gameObject.transform.parent.GetComponent<MainBoardScript>();
        boxes = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            float x = (i % 3 - 1) * 2 + gameObject.transform.position.x;
            float y = ((i - i % 3) / 3 - 1) * -2 + gameObject.transform.position.y;
            boxes[i] = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity, gameObject.transform);
            boxes[i].name = "Box " + i;
            boxes[i].GetComponent<BoxScript>().SetBoxNumber(i);
        }
    }

    public void SetBoxLetter(int boxNumber, Letter newLetter)
    {
        boxes[boxNumber].GetComponent<BoxScript>().SetLetter(newLetter);
    }

    public Letter GetBoxLetter(int boxNumber)
    {
        return boxes[boxNumber].GetComponent<BoxScript>().GetLetter();
    }

    public void OnBoxClicked(int boxNumber)
    {
        parentScript.OnBoxClicked(boxNumber);
    }
}
