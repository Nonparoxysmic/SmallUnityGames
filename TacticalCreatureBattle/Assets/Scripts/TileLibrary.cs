using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLibrary : MonoBehaviour
{
    public static TileLibrary Instance { get; private set; }

    public Tile[] DevelopmentTiles;

    void Awake()
    {
        Instance = this;
        if (DevelopmentTiles.Length == 0)
        {
            this.Error($"{nameof(DevelopmentTiles)} array is empty.");
            return;
        }
    }

    public static (Tile, Color) GetTile(uint data)
    {
        // TODO: Add more tiles.
        return (GetDevelopmentTile(data), Color.white);
    }

    public static Tile GetDevelopmentTile(uint index)
    {
        return Instance.DevelopmentTiles[index % Instance.DevelopmentTiles.Length];
    }
}
