using System;
using UnityEngine;

public class TESTBoardCreator : MonoBehaviour
{
    [SerializeField] GameObject gameBoardPrefab;
    public int boardWidth = 3;
    public int boardHeight = 3;
    public int victoryNum = 3;

    void Start()
    {
        if (gameBoardPrefab == null)
        {
            Debug.LogError(gameObject.name + ": Prefab reference not set in the Inspector.");
            gameBoardPrefab = new GameObject();
        }
        int baseX = (int)Math.Round(gameObject.transform.parent.transform.position.x);
        int baseY = (int)Math.Round(gameObject.transform.parent.transform.position.y);
        GameObject newObject = Instantiate(gameBoardPrefab, new Vector3(baseX, baseY, 100), Quaternion.identity, gameObject.transform.parent.transform);
        newObject.name = "Game Board Test";
        newObject.GetComponent<BoardCreator>().CreateGameBoard(boardWidth, boardHeight, victoryNum);
    }
}
