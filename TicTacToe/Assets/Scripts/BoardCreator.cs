using System;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    
    void Start()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Box Prefab reference is not set in the Inspector.");
        }
    }

    public void CreateGameBoard(int width, int height, int victoryNumber)
    {
        if ((width < 1) || (height < 1) || (victoryNumber < 1))
        {
            Debug.LogError("Invalid parameters in CreateGameBoard().");
            return;
        }

        int baseX = (int)Math.Round(gameObject.transform.position.x) + 1 - width;
        int baseY = (int)Math.Round(gameObject.transform.position.y) + 1 - height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject createdBox = Instantiate(boxPrefab, new Vector3(baseX + x * 2, baseY + y * 2, 0), Quaternion.identity, gameObject.transform);
                createdBox.name = "Box (" + x + "," + y + ")";
            }
        }
    }
}
