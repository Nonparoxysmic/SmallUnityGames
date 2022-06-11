using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    static readonly Vector3Int[] adjacentDirections = new Vector3Int[]
    {
        new Vector3Int(-1,  0,  0),
        new Vector3Int(-1, -1,  0),
        new Vector3Int( 0, -1,  0),
        new Vector3Int( 1, -1,  0),
        new Vector3Int( 1,  0,  0),
        new Vector3Int( 1,  1,  0),
        new Vector3Int( 0,  1,  0),
        new Vector3Int(-1,  1,  0)
    };

    [SerializeField] Tilemap backgroundTilemap;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile collisionTile;
    [SerializeField] int randomSeed;
    [SerializeField] bool randomizeSeed;
    [SerializeField] Vector2Int noiseScale;
    [SerializeField] Vector3Int playerChunk;
    [SerializeField] Tile[] tiles;

    GameMaster gm;

    readonly int chunkSize = 16;
    Noise noise;
    readonly Queue<Vector3Int> chunksToGenerate = new Queue<Vector3Int>();
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

        if (randomizeSeed) { randomSeed = Random.Range(int.MinValue, int.MaxValue); }
        noise = new Noise(randomSeed, noiseScale.x, noiseScale.y);
        playerChunk = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        GenerateChunk(0, 0);
    }

    void FixedUpdate()
    {
        Vector3 playerPos = gm.PlayerPosition();
        Vector3Int currentPlayerChunk = ChunkOffset(playerPos);
        bool changedChunk = false;
        if (currentPlayerChunk != playerChunk)
        {
            changedChunk = true;
            playerChunk = currentPlayerChunk;
        }
        if (changedChunk)
        {
            foreach (Vector3Int adjacentDirection in adjacentDirections)
            {
                Vector3Int chunk = playerChunk + adjacentDirection;
                if (backgroundTilemap.GetTile(chunkSize * chunk) == null)
                {
                    chunksToGenerate.Enqueue(chunk);
                }
            }
        }
        if (chunksToGenerate.Count > 0)
        {
            GenerateChunk(chunksToGenerate.Dequeue());
        }

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

    void GenerateChunk(Vector3Int offsetVector)
    {
        GenerateChunk(offsetVector.x, offsetVector.y);
    }

    void GenerateChunk(int offsetX, int offsetY)
    {
        Vector2Int chunkPos = new Vector2Int(chunkSize * offsetX, chunkSize * offsetY);
        Vector2Int tilePos = Vector2Int.zero;
        for (int deltaY = 0; deltaY < chunkSize; deltaY++)
        {
            for (int deltaX = 0; deltaX < chunkSize; deltaX++)
            {
                tilePos.x = chunkPos.x + deltaX;
                tilePos.y = chunkPos.y + deltaY;

                // Temporary tile generation:
                int temp = (int)(4 * noise.Value(tilePos.x, tilePos.y));
                if (Mathf.Abs(tilePos.x) < 2 && Mathf.Abs(tilePos.y) < 2)
                {
                    temp = Mathf.Max(temp, 1);
                }
                SetTile(backgroundTilemap, tilePos.x, tilePos.y, tiles[temp], temp == 0);
            }
        }
    }

    Vector3Int ChunkOffset(Vector3 position)
    {
        int x = position.x.Floor(chunkSize) / chunkSize;
        int y = position.y.Floor(chunkSize) / chunkSize;
        return new Vector3Int(x, y, 0);
    }
}
