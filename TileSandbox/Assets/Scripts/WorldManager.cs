using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Tilemap backgroundTilemap;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile collisionTile;

    GameMaster gm;

    readonly Queue<(int, int)> collidersToAdd = new Queue<(int, int)>();

    void Start()
    {
        gm = GetComponent<GameMaster>();
        if (gm is null)
        {
            this.Error("Missing or unavailable Game Master");
            return;
        }
        if (backgroundTilemap is null)
        {
            this.Error("Background Tilemap reference not set in Inspector.");
            return;
        }
        if (collisionTilemap is null)
        {
            this.Error("Collision Tilemap reference not set in Inspector.");
            return;
        }
        if (collisionTile is null)
        {
            this.Error("Collision Tile reference not set in Inspector.");
            return;
        }
    }

    void FixedUpdate()
    {
        Vector3 playerPos = gm.PlayerPosition();
        int pending = collidersToAdd.Count;
        for (int i = 0; i < pending; i++)
        {
            (int, int) target = collidersToAdd.Dequeue();
            float xDiff = Mathf.Abs(playerPos.x - target.Item1);
            float yDiff = Mathf.Abs(playerPos.y - target.Item2);
            if (xDiff > 1 || yDiff > 1)
            {
                Vector3Int targetPos = new Vector3Int(target.Item1, target.Item2, 0);
                collisionTilemap.SetTile(targetPos, collisionTile);
            }
            else
            {
                collidersToAdd.Enqueue(target);
            }
        }
    }

    void SetTile(Tilemap tilemap, int x, int y, Tile tile, bool collision)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        tilemap.SetTile(position, tile);
        if (collision)
        {
            if (!collidersToAdd.Contains((x, y)))
            {
                collidersToAdd.Enqueue((x, y));
            }
        }
        else
        {
            collisionTilemap.SetTile(position, null);
            collidersToAdd.Remove((x, y));
        }
    }
}
