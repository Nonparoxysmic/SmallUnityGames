using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Tilemap fogTilemap;

    [HideInInspector] public Vector3Int currentTilePos;

    CollisionMonitor collisionMonitor;
    GameClock gameClock;
    Pathfinding pathfinding;

    Vector3Int lookTilePos;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
        pathfinding = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();
    }

    void Start()
    {
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
        gameClock.onNonPlayerTick.AddListener(NonPlayerUpdate);
    }

    void NonPlayerUpdate()
    {
        if (fogTilemap.GetTile(currentTilePos) == null) return;
        List<Vector3Int> bestMoves = pathfinding.GetBestMovesTowardPlayer(currentTilePos);
        foreach (Vector3Int move in bestMoves)
        {
            lookTilePos = currentTilePos + move;
            if (fogTilemap.GetTile(lookTilePos) == null || !collisionMonitor.TileIsEmpty(lookTilePos)) continue;
            currentTilePos = lookTilePos;
            transform.position = new Vector3(lookTilePos.x + 0.5f, lookTilePos.y + 0.5f, 0);
            break;
        }
    }
}
