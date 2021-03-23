using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMonitor : MonoBehaviour
{
    [SerializeField] EnemyBehavior testEnemy;
    [SerializeField] Tilemap wallTilemap;

    [HideInInspector] public RectInt levelBoundary;

    PlayerMovement playerMovement;

    Vector3Int targetTilePosition;

    void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        levelBoundary = new RectInt(wallTilemap.origin.x, wallTilemap.origin.y, wallTilemap.size.x, wallTilemap.size.y);
    }

    public bool TileIsEmpty(Vector3Int tilePosition)
    {
        if (testEnemy.currentTilePos == tilePosition) return false;

        if (wallTilemap.GetTile(tilePosition) != null) return false;

        if (playerMovement.currentTilePos == tilePosition) return false;
        if (playerMovement.isMoving)
        {
            targetTilePosition = new Vector3Int((int)Math.Floor(playerMovement.targetPos.x), (int)Math.Floor(playerMovement.targetPos.y), 0);
            if (targetTilePosition == tilePosition) return false;
        }
        return true;
    }

    public bool TileIsPath(Vector3Int tilePosition)
    {
        if (wallTilemap.GetTile(tilePosition) != null) return false;
        return true;
    }

    public bool LevelContainsPosition(Vector3Int tilePosition)
    {
        if (tilePosition.x < levelBoundary.x || tilePosition.y < levelBoundary.y 
            || tilePosition.x >= levelBoundary.x + levelBoundary.width 
            || tilePosition.y >= levelBoundary.y + levelBoundary.height) return false;
        return true;
    }

    public bool[,] GetPathArray()
    {
        bool[,] paths = new bool[levelBoundary.width, levelBoundary.height];
        Vector3Int lookTilePos;
        for (int x = 0; x < levelBoundary.width; x++)
        {
            for (int y = 0; y < levelBoundary.height; y++)
            {
                lookTilePos = new Vector3Int(levelBoundary.x + x, levelBoundary.y + y, 0);
                if (TileIsPath(lookTilePos)) paths[x, y] = true;
            }
        }
        return paths;
    }
}
