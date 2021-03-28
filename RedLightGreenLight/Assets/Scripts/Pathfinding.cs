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
    List<PathNode> closedList;
    Vector3Int levelOffset;
    Vector3Int lookTilePos;
    List<PathNode> neighborList;
    List<PathNode> openList;
    PathNode[,] pathNodes;
    bool[,] tileIsPath;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        bestMoves = new List<Vector3Int>();
    }

    void Start()
    {
        tileIsPath = collisionMonitor.GetPathArray();
        levelOffset = new Vector3Int(collisionMonitor.levelBoundary.x, collisionMonitor.levelBoundary.y, 0);
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
        // Ensure that input parameters are valid.
        if (!collisionMonitor.LevelContainsPosition(startTilePos) || !collisionMonitor.LevelContainsPosition(endTilePos))
        {
            return int.MaxValue;
        }
        if (!collisionMonitor.TileIsPath(startTilePos) || !collisionMonitor.TileIsPath(endTilePos))
        {
            return int.MaxValue;
        }
        if (startTilePos == endTilePos) return 0;
        // Adjust positions to match array indices.
        startTilePos -= levelOffset;
        endTilePos -= levelOffset;

        // A* Pathfinding

        // Initialize array of nodes.
        pathNodes = new PathNode[tileIsPath.GetLength(0), tileIsPath.GetLength(1)];
        for (int x = 0; x < pathNodes.GetLength(0); x++)
        {
            for (int y = 0; y < pathNodes.GetLength(1); y++)
            {
                pathNodes[x, y] = new PathNode(x, y, tileIsPath[x, y]);
                if (x == startTilePos.x && y == startTilePos.y)
                {
                    pathNodes[x, y].stepsFromStart = 0;
                    pathNodes[x, y].distanceToEnd = Math.Abs(startTilePos.x - endTilePos.x) + Math.Abs(startTilePos.y - endTilePos.y);
                    pathNodes[x, y].score = pathNodes[x, y].stepsFromStart + pathNodes[x, y].distanceToEnd;
                }
            }
        }
        // Initialize node lists.
        openList = new List<PathNode>();
        closedList = new List<PathNode>();
        openList.Add(pathNodes[startTilePos.x, startTilePos.y]);
        // While there are nodes to be analyzed...
        while (openList.Count > 0)
        {
            // Get a node on the open list with the lowest score
            PathNode currentNode = null;
            int lowestScore = int.MaxValue;
            foreach (PathNode node in openList)
            {
                if (node.score < lowestScore)
                {
                    lowestScore = node.score;
                    currentNode = node;
                }
            }
            // If the current node is the goal node, return the path length.
            if (currentNode.x == endTilePos.x && currentNode.y == endTilePos.y)
            {
                return currentNode.stepsFromStart;
            }
            // Move current node from open list to closed list.
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            // Make a list of the neighbors of the current node.
            neighborList = new List<PathNode>();
            if (currentNode.x > 0) neighborList.Add(pathNodes[currentNode.x - 1, currentNode.y]);
            if (currentNode.y > 0) neighborList.Add(pathNodes[currentNode.x, currentNode.y - 1]);
            if (currentNode.x < pathNodes.GetLength(0) - 1) neighborList.Add(pathNodes[currentNode.x + 1, currentNode.y]);
            if (currentNode.y < pathNodes.GetLength(1) - 1) neighborList.Add(pathNodes[currentNode.x, currentNode.y + 1]);
            // For each neighbor node...
            foreach (PathNode neighbor in neighborList)
            {
                // If the neighbor node is not a path, or if it has already been fully analyzed, skip it.
                if (!neighbor.isPath || closedList.Contains(neighbor)) continue;
                // If the neighbor node is not on the open list, or if the path through the current node
                // is shorter than the neighbor node's previously considered path...
                if (!openList.Contains(neighbor) || currentNode.stepsFromStart + 1 < neighbor.stepsFromStart)
                {
                    // Recalculate the neighbor node's values.
                    neighbor.stepsFromStart = currentNode.stepsFromStart + 1;
                    neighbor.distanceToEnd = Math.Abs(neighbor.x - endTilePos.x) + Math.Abs(neighbor.y - endTilePos.y);
                    neighbor.score = neighbor.stepsFromStart + neighbor.distanceToEnd;
                    // Add the neighbor to the open list if it hasn't been added yet.
                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }
        // The goal node was not found, so there is no path.
        return int.MaxValue;
    }
}

class PathNode
{
    public int x;
    public int y;
    public bool isPath;
    public int stepsFromStart;
    public int distanceToEnd;
    public int score;

    public PathNode(int x, int y, bool isPath)
    {
        this.x = x;
        this.y = y;
        this.isPath = isPath;
        stepsFromStart = int.MaxValue;
        distanceToEnd = int.MaxValue;
        score = int.MaxValue;
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
