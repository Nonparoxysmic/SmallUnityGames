using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    readonly Vector3Int[] moveDirections =
        {
            new Vector3Int(-1,  0,  0),
            new Vector3Int( 1,  0,  0),
            new Vector3Int( 0, -1,  0),
            new Vector3Int( 0,  1,  0)
        };

    CollisionMonitor collisionMonitor;
    PlayerMovement playerMovement;

    List<Vector3Int> bestMoves;
    Vector3Int lookTilePos;
    bool[,] pathArray;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        bestMoves = new List<Vector3Int>();
    }

    void Start()
    {
        pathArray = collisionMonitor.GetPathArray();
    }

    public List<Vector3Int> GetBestMovesTowardPlayer(Vector3Int startTilePos)
    {
        bestMoves.Clear();
        int shortestPathLength = int.MaxValue - 1;
        foreach (Vector3Int moveDir in moveDirections)
        {
            lookTilePos = startTilePos + moveDir;
            if (collisionMonitor.TileIsPath(lookTilePos))
            {
                int length = GetPathLength(lookTilePos, playerMovement.currentTilePos);
                if (length < shortestPathLength)
                {
                    bestMoves.Clear();
                    bestMoves.Add(lookTilePos);
                    shortestPathLength = length;
                }
                else if (length == shortestPathLength) bestMoves.Add(lookTilePos);
            }
        }
        bestMoves.Shuffle();
        return bestMoves;
    }

    int GetPathLength(Vector3Int startTilePos, Vector3Int endTilePos)
    {
        if (!collisionMonitor.LevelContainsPosition(startTilePos) || !collisionMonitor.LevelContainsPosition(endTilePos))
        {
            return int.MaxValue;
        }

        // TODO: A* pathfinding

        return 2;  // TEMPORARY RETURN
    }
}

public static class ListShufflingExtension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
