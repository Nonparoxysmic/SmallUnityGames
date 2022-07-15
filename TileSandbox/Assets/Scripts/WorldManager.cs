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
    [SerializeField] int randomSeed;
    [SerializeField] bool randomizeSeed;
    [SerializeField] Vector3Int playerChunk;

    GameMaster gm;
    Tile collisionTile;
    TileCollection tileCollection;

    readonly int chunkSize = 16;
    Noise biomeNoise;
    Noise biomeType;
    Noise objectNoise;
    readonly HashSet<(int, int)> holes = new HashSet<(int, int)>();
    readonly Queue<Vector3Int> chunksToGenerate = new Queue<Vector3Int>();
    readonly Queue<(int, int)> collidersToAdd = new Queue<(int, int)>();
    readonly Queue<(int, int)> pendingTileUpdates = new Queue<(int, int)>();

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
        collisionTile = ScriptableObject.CreateInstance<Tile>();
        collisionTile.sprite = Utilities.BlankSquareSprite(4, Color.white);
        collisionTile.colliderType = Tile.ColliderType.Sprite;

        if (randomizeSeed) { randomSeed = Random.Range(int.MinValue, int.MaxValue); }
        biomeType = new Noise(randomSeed, 11, 11);
        biomeNoise = new Noise(randomSeed, 5, 5);
        objectNoise = new Noise(randomSeed, 1, 1);
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

        if (pendingTileUpdates.Count > 0)
        {
            (int, int) tileCoords = pendingTileUpdates.Dequeue();
            if (holes.Contains(tileCoords))
            {
                Vector3Int tilePosition = new Vector3Int(tileCoords.Item1, tileCoords.Item2, 0);
                for (int i = 0; i < adjacentDirections.Length; i += 2)
                {
                    Vector3Int pos = tilePosition + adjacentDirections[i];
                    if (backgroundTilemap.GetTile(pos).name == tileCollection.WaterTileName)
                    {
                        SetTile(backgroundTilemap, tileCoords.Item1, tileCoords.Item2, GetTile(0));
                        holes.Remove(tileCoords);
                        for (int j = 0; j < adjacentDirections.Length; j += 2)
                        {
                            pendingTileUpdates.Enqueue((tileCoords.Item1 + adjacentDirections[j].x,
                                tileCoords.Item2 + adjacentDirections[j].y));
                        }
                        break;
                    }
                }
            }
        }
    }

    void SetTile(Tilemap tilemap, int x, int y, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
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

    public void SetObjectTile(int x, int y, int index)
    {
        SetTile(objectTilemap, x, y, GetTile(index));
    }

    public void SetObjectTile(int x, int y, int index, bool collision)
    {
        SetTile(objectTilemap, x, y, GetTile(index), collision);
    }

    public void ClearObjectTile(int x, int y)
    {
        SetTile(objectTilemap, x, y, null);
    }

    public void ClearObjectTile(int x, int y, bool collision)
    {
        SetTile(objectTilemap, x, y, null, collision);
    }

    public void SetBackgroundTile(int x, int y, int index)
    {
        SetTile(backgroundTilemap, x, y, GetTile(index));
    }

    public void SetBackgroundTile(int x, int y, int index, bool collision)
    {
        SetTile(backgroundTilemap, x, y, GetTile(index), collision);
    }

    public int GetBackgroundTileIndex(int x, int y)
    {
        return GetTileIndex(backgroundTilemap, x, y);
    }

    public int GetObjectTileIndex(int x, int y)
    {
        return GetTileIndex(objectTilemap, x, y);
    }

    int GetTileIndex(Tilemap tilemap, int x, int y)
    {
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        return tile is null ? -1 : tileCollection.GetTileIndex(tile.name);
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
        float biomeValue = biomeType.Value(x, y) + 0.25f * biomeNoise.Value(x, y) - 0.125f;
        float objectValue = objectNoise.Value(x, y);

        // Near the spawn point, force land and disallow objects.
        int stepsFromSpawn = Mathf.Abs(x) + Mathf.Abs(y);
        float spawnRadius = 16f;
        if (stepsFromSpawn < spawnRadius)
        {
            biomeValue = Mathf.Max(biomeValue, (spawnRadius - stepsFromSpawn) / spawnRadius);
            if (stepsFromSpawn < 3)
            {
                objectValue = 0;
            }
        }

        if (biomeValue < 0.5)
        {
            // Ocean Tile
            SetTile(backgroundTilemap, x, y, GetTile(0), true);
            if (objectValue >= 0.98)
            {
                SetTile(objectTilemap, x, y, GetTile(5), true);
            }
        }
        else if (biomeValue.InRange(0.5, 0.667))
        {
            // Beach Tile
            SetTile(backgroundTilemap, x, y, GetTile(1));
            if (objectValue >= 0.96)
            {
                SetTile(objectTilemap, x, y, GetTile(4), true);
            }
            else if (objectValue >= 0.92)
            {
                SetTile(objectTilemap, x, y, GetTile(5), true);
            }
        }
        else
        {
            // Land Tile
            SetTile(backgroundTilemap, x, y, GetTile(2));
            if (objectValue >= 0.92)
            {
                SetTile(objectTilemap, x, y, GetTile(4), true);
            }
            else if (objectValue >= 0.84)
            {
                SetTile(backgroundTilemap, x, y, GetTile(3), true);
                SetTile(objectTilemap, x, y, GetTile(5), true);
            }
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

    public Sprite GetTileSprite(int index)
    {
        return GetTile(index).sprite;
    }

    Tile GetHoleTile()
    {
        return tileCollection.HoleBase();
    }

    Tile GetHoleTile(int index)
    {
        return tileCollection.GetHoleTile(index);
    }

    public void AddHole(int x, int y)
    {
        if (!holes.Add((x, y))) { return; }

        if (holes.Contains((x, y - 1)))
        {
            SetTile(backgroundTilemap, x, y - 1, GetHoleTile());
        }

        if (holes.Contains((x, y + 1)))
        {
            SetTile(backgroundTilemap, x, y, GetHoleTile(), true);
        }
        else
        {
            string nameOfTileAbove = backgroundTilemap.GetTile(new Vector3Int(x, y + 1, 0)).name;
            int indexOfTileAbove = tileCollection.GetTileIndex(nameOfTileAbove);
            SetTile(backgroundTilemap, x, y, GetHoleTile(indexOfTileAbove), true);
        }

        pendingTileUpdates.Enqueue((x, y));
    }

    public bool RemoveHole(int x, int y, int tileIndex)
    {
        if (!holes.Remove((x, y))) { return false; }

        if (holes.Contains((x, y - 1)))
        {
            SetTile(backgroundTilemap, x, y - 1, GetHoleTile(tileIndex));
        }
        SetTile(backgroundTilemap, x, y, GetTile(tileIndex), false);
        return true;
    }

    public bool IsHole(int x, int y)
    {
        return holes.Contains((x, y));
    }
}
