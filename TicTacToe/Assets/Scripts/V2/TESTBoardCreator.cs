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
        GameObject newObject = Instantiate(gameBoardPrefab, new Vector3(0, 0, 100), Quaternion.identity, gameObject.transform.parent.transform);
        newObject.name = "Game Board Test";
        newObject.GetComponent<BoardCreator>().CreateGameBoard(boardWidth, boardHeight, victoryNum);
    }
}
