using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Tilemap backgroundTilemap;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile collisionTile;

    void Start()
    {
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

    void SetBackgroundTile(int x, int y, Tile tile, bool collision)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        backgroundTilemap.SetTile(position, tile);
        if (collision)
        {
            collisionTilemap.SetTile(position, collisionTile);
        }
        else
        {
            collisionTilemap.SetTile(position, null);
        }
    }
}
