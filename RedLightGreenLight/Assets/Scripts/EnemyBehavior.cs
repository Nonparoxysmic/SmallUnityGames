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
    PlayerMovement playerMovement;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
        pathfinding = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
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
        int distanceToPlayer = Math.Abs(currentTilePos.x - playerMovement.currentTilePos.x) + Math.Abs(currentTilePos.y - playerMovement.currentTilePos.y);
        if (distanceToPlayer == 1)
        {
            Debug.Log("Enemy caught player.");
            return;
        }
        List<Vector3Int> bestMoves = pathfinding.GetBestMovesTowardPlayer(currentTilePos);
        foreach (Vector3Int movePos in bestMoves)
        {
            if (fogTilemap.GetTile(movePos) == null || !collisionMonitor.TileIsEmpty(movePos)) continue;
            currentTilePos = movePos;
            transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
            break;
        }
    }
}
