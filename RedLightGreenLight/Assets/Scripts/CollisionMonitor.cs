using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionMonitor : MonoBehaviour
{
    [SerializeField] Tilemap wallTilemap;

    PlayerMovement playerMovement;
    Vector3Int targetTilePosition;

    void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public bool TileIsEmpty(Vector3Int tilePosition)
    {
        if (wallTilemap.GetTile(tilePosition) != null) return false;
        if (playerMovement.currentTilePos == tilePosition) return false;
        if (playerMovement.isMoving)
        {
            targetTilePosition = new Vector3Int((int)Math.Floor(playerMovement.targetPos.x), (int)Math.Floor(playerMovement.targetPos.y), 0);
            if (targetTilePosition == tilePosition) return false;
        }
        return true;
    }
}
