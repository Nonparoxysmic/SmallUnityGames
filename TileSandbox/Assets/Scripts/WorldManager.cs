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
    [SerializeField] Tilemap objectTilemap;
    [SerializeField] Tile collisionTile;
    [SerializeField] int randomSeed;
    [SerializeField] bool randomizeSeed;
    [SerializeField] Vector3Int playerChunk;

    GameMaster gm;
    TileCollection tileCollection;

    readonly int chunkSize = 16;
    Noise noise1;
    Noise noise3;
    Noise noiseWide;
    readonly Queue<Vector3Int> chunksToGenerate = new Queue<Vector3Int>();
    readonly Queue<(int, int)> collidersToAdd = new Queue<(int, int)>();

    void Start()
    {
        gm = GetComponent<GameMaster>();
        tileCollection = GetComponent<TileCollection>();
        if (gm is null)
        {
            this.Error("Missing or unavailable Game Master");
            return;
        }
        if (tileCollection is null)
        {
            this.Error("Missing or unavailable Tile Collection");
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
        noise1 = new Noise(randomSeed, 1, 1);
        noise3 = new Noise(randomSeed, 3, 3);
        noiseWide = new Noise(randomSeed, 29, 29);
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
                GenerateTile(tilePos.x, tilePos.y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        // TODO: Redo this method.
        bool spawnArea = false;
        float noise3Value = noise3.Value(x, y);
        float noiseWideValue = noiseWide.Value(x, y);
        int temp;
        if (noiseWideValue < 0.25)
        {
            temp = (int)(4 * noise3Value * noiseWideValue);
        }
        else if (noiseWideValue < 0.5)
        {
            temp = (int)(2 * (noise3Value + noiseWideValue));
        }
        else
        {
            temp = (int)(4 * noise3Value);
        }
        if (Mathf.Abs(x) < 2 && Mathf.Abs(y) < 2)
        {
            spawnArea = true;
            temp = Mathf.Max(temp, 1);
        }
        SetTile(backgroundTilemap, x, y, GetTile(temp), temp == 0);
        if (spawnArea) { return; }
        float objectNoise = noise1.Value(x, y);
        if (temp == 1 && objectNoise < 0.0625)
        {
            SetTile(objectTilemap, x, y, GetTile(4), true);
        }
        else if (temp > 0 && 0.0625 <= objectNoise && objectNoise < 0.0833)
        {
            SetTile(objectTilemap, x, y, GetTile(5), true);
        }
    }

    Vector3Int ChunkOffset(Vector3 position)
    {
        int x = position.x.Floor(chunkSize) / chunkSize;
        int y = position.y.Floor(chunkSize) / chunkSize;
        return new Vector3Int(x, y, 0);
    }

    Tile GetTile(int index)
    {
        return tileCollection.GetTile(index);
    }
}
