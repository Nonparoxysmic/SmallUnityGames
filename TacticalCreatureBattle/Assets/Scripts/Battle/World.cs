using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    // This component creates and manages all of the GameObjects (such as Tilemaps and Sprites)
    // that represent the battle map and the creatures.

    Grid _grid;
    Map _map;

    void Awake()
    {
        // Create Tilemap Grid.
        GameObject gridGameObject = new GameObject();
        gridGameObject.transform.parent = transform;
        gridGameObject.name = "Grid";
        _grid = gridGameObject.AddComponent<Grid>();
    }

    public void CreateWorld(WorldLoader worldLoader, CreatureData creatureData)
    {
        _map = worldLoader.Current;
        if (_map == null)
        {
            this.Error("Current world is null.");
            return;
        }
        // TODO: Create world GameObjects. (E.g. Tilemaps and Sprites)
        // TODO: Create creature sprites.
    }

    void CreateTilemap(string name, uint[,] data)
    {
        GameObject tilemapGameObject = new GameObject();
        tilemapGameObject.transform.parent = _grid.transform;
        tilemapGameObject.name = name;
        Tilemap tilemap = tilemapGameObject.AddComponent<Tilemap>();
        tilemapGameObject.AddComponent<TilemapRenderer>();
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                (Tile tile, Color color) = TileLibrary.GetTile(data[x, y]);
                Vector3Int position = new Vector3Int(x, -y, 0);
                tilemap.SetTile(position, tile);
                tilemap.SetColor(position, color);
            }
        }
    }
}
