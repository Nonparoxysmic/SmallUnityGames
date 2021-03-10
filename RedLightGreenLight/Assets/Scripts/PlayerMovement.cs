using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    const float WalkSpeed = 2.5f;

    GameClock gameClock;
    [HideInInspector] public Vector3Int currentTilePos;

    bool isMoving;
    int moveCooldown;
    int deltaX;
    int deltaY;

    void Awake()
    {
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
    }

    void Start()
    {
        gameClock.onPlayerTick.AddListener(PlayerUpdate);
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, 0);
    }

    void Update()
    {
        if (isMoving)
        {
            currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);

            float newX = transform.position.x + deltaX * WalkSpeed * Time.deltaTime;
            float newY = transform.position.y + deltaY * WalkSpeed * Time.deltaTime;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    public void PlayerUpdate()
    {
        if (moveCooldown > 0)
        {
            moveCooldown--;
            return;
        }
        isMoving = false;
        deltaX = (int)Input.GetAxisRaw("East") - (int)Input.GetAxisRaw("West");
        deltaY = (int)Input.GetAxisRaw("North") - (int)Input.GetAxisRaw("South");
        if (deltaX == 0 ^ deltaY == 0)
        {
            isMoving = true;
            moveCooldown = 1;
        }
    }
}
