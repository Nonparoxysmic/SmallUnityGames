using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //CollisionMonitor collisionMonitor;
    //PlayerMovement playerMovement;

    List<Vector3Int> bestMoves;

    void Awake()
    {
        //collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        //playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        bestMoves = new List<Vector3Int>();
    }

    public List<Vector3Int> GetBestMovesTowardPlayer(Vector3Int startTilePos)
    {
        bestMoves.Clear();

        // Temporary placeholder
        bestMoves.Add(new Vector3Int(-1, 0, 0));
        bestMoves.Add(new Vector3Int(1, 0, 0));
        bestMoves.Add(new Vector3Int(0, -1, 0));
        bestMoves.Add(new Vector3Int(0, 1, 0));

        bestMoves.Shuffle();
        return bestMoves;
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
