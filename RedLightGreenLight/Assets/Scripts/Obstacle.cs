using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    CollisionMonitor collisionMonitor;

    Vector3Int currentTilePos;

    void Awake()
    {
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
    }

    void Start()
    {
        currentTilePos = new Vector3Int((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y), 0);
        transform.position = new Vector3(currentTilePos.x + 0.5f, currentTilePos.y + 0.5f, transform.position.z);
        collisionMonitor.AddObstaclePosition(currentTilePos);
    }
}
