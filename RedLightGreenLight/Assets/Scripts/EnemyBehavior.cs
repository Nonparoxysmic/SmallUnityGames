using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] Tilemap fogTilemap;
    public int detectionRange;
    public int moveDelay;
    public int wakeCountdown;

    [HideInInspector] public Vector3Int currentTilePos;

    CollisionMonitor collisionMonitor;
    EnemyManager enemyManager;
    GameClock gameClock;
    Pathfinding pathfinding;
    PlayerMovement playerMovement;
    PlayerVision playerVision;

    int moveCountdown;
    int pathLengthToPlayer;
    Vector3Int lookTilePos;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        enemyManager = transform.parent.gameObject.GetComponent<EnemyManager>();
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
        pathfinding = GameObject.Find("Pathfinder").GetComponent<Pathfinding>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerVision = GameObject.Find("Player").GetComponent<PlayerVision>();
    }

    void Start()
    {
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
        collisionMonitor.UpdateEnemyPosition(gameObject, currentTilePos);
        gameClock.onNonPlayerTick.AddListener(NonPlayerUpdate);
    }

    void NonPlayerUpdate()
    {
        if (wakeCountdown > 0)
        {
            if (fogTilemap.GetTile(currentTilePos) == null) wakeCountdown--;
            return;
        }
        if ((fogTilemap.GetTile(currentTilePos) == null && !playerVision.isBlinking) || enemyManager.playerIsCaught) return;
        if (moveCountdown > 0)
        {
            moveCountdown--;
            return;
        }
        int distanceToPlayer = Math.Abs(currentTilePos.x - playerMovement.currentTilePos.x) + Math.Abs(currentTilePos.y - playerMovement.currentTilePos.y);
        if (distanceToPlayer == 1)
        {
            enemyManager.onPlayerCaught.Invoke();
            return;
        }
        if (distanceToPlayer > detectionRange) return;
        List<Vector3Int> bestMoves = pathfinding.GetBestMovesTowardPlayer(currentTilePos, out pathLengthToPlayer);
        if (pathLengthToPlayer > detectionRange) return;
        foreach (Vector3Int moveDirection in bestMoves)
        {
            lookTilePos = currentTilePos + moveDirection;
            if ((fogTilemap.GetTile(lookTilePos) == null && !playerVision.isBlinking) || !collisionMonitor.TileIsEmpty(lookTilePos)) continue;
            currentTilePos = lookTilePos;
            transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
            collisionMonitor.UpdateEnemyPosition(gameObject, currentTilePos);
            moveCountdown += moveDelay;
            break;
        }
    }
}
