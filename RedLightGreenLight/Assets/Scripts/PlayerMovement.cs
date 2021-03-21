using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    const float WalkSpeed = 2.5f;

    [HideInInspector] public Vector3Int currentTilePos;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public Vector3 targetPos;

    CollisionMonitor collisionMonitor;
    GameClock gameClock;

    Vector3Int lookTilePos;
    Vector2Int moveInput;
    bool movingFrame;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
    }

    void Start()
    {
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
        targetPos = transform.position;
        gameClock.onPlayerTick.AddListener(PlayerUpdate);
    }

    void Update()
    {
        if (!transform.position.Equals(targetPos))
        {
            Vector3 difference = targetPos - transform.position;
            Vector3 step = difference / difference.magnitude * WalkSpeed * Time.deltaTime;
            if (step.magnitude < difference.magnitude)
            {
                transform.position += step;
            }
            else
            {
                transform.position = targetPos;
                isMoving = false;
            }
            currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        }
    }

    public void PlayerUpdate()
    {
        if (movingFrame)
        {
            movingFrame = false;
            return;
        }
        if (!transform.position.Equals(targetPos))
        {
            transform.position = targetPos;
            isMoving = false;
            currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        }
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x == 0 ^ moveInput.y == 0)
        {
            lookTilePos = new Vector3Int(currentTilePos.x + moveInput.x, currentTilePos.y + moveInput.y, 0);
            if (collisionMonitor.TileIsEmpty(lookTilePos))
            {
                isMoving = true;
                targetPos = new Vector3(lookTilePos.x + 0.5f, lookTilePos.y + 0.5f, 0);
                movingFrame = true;
            }
        }
    }
}
