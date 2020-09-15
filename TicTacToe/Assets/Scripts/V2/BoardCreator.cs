using System;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    BoardState boardState;
    
    void Start()
    {
        if (boxPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Box Prefab reference is not set in the Inspector.");
        }
        boardState = GetComponent<BoardState>();
        if (boardState == null)
        {
            Debug.LogError(gameObject.name + ": Board State component not found.");
        }
        Debug.Log("Start");
    }

    public void CreateGameBoard(int width, int height, int victoryNumber)
    {
        if ((width < 1) || (height < 1) || (victoryNumber < 1))
        {
            Debug.LogError(gameObject.name + ": Invalid parameters in CreateGameBoard().");
            return;
        }

        Debug.Log("CreateGameBoard");
        // This is being called before Start()
        //boardState.InitializeBoardState(width, height, victoryNumber);

        int baseX = (int)Math.Round(gameObject.transform.position.x) + 1 - width;
        int baseY = (int)Math.Round(gameObject.transform.position.y) + 1 - height;
        int baseZ = (int)Math.Round(gameObject.transform.position.z) - 1;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject createdBox = Instantiate(boxPrefab, new Vector3(baseX + x * 2, baseY + y * 2, baseZ), Quaternion.identity, gameObject.transform);
                createdBox.name = "Box (" + x + "," + y + ")";
            }
        }
    }
}
