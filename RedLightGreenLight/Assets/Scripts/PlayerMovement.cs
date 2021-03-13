using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    const float WalkSpeed = 2.5f;

    [SerializeField] Tilemap wallTilemap;

    [HideInInspector] public Vector3Int currentTilePos;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public Vector3 targetPos;

    GameClock gameClock;
    Vector2Int moveInput;
    bool movingFrame;

    void Awake()
    {
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
    }

    void Start()
    {
        gameClock.onPlayerTick.AddListener(PlayerUpdate);
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        targetPos = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
        transform.position = targetPos;
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
            if (wallTilemap.GetTile(new Vector3Int(currentTilePos.x + moveInput.x, currentTilePos.y + moveInput.y, 0)) == null)
            {
                targetPos = new Vector3(currentTilePos.x + moveInput.x + 0.5f, currentTilePos.y + moveInput.y + 0.5f, 0);
                isMoving = true;
                movingFrame = true;
            }
        }
    }
}
