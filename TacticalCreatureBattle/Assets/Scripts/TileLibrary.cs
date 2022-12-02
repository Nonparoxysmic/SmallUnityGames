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

    public static Tile GetDevelopmentTile(int index)
    {
        if (index < 0)
        {
            Debug.LogError($"{nameof(GetDevelopmentTile)}: Negative index passed as argument.");
            index = 0;
        }
        return Instance.DevelopmentTiles[index % Instance.DevelopmentTiles.Length];
    }
}
