using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerVision : MonoBehaviour
{
    [SerializeField] Tilemap fogTilemap;
    int currentTileX;
    int currentTileY;

    [SerializeField] Tile whiteTile;

    void Update()
    {
        currentTileX = (int)Math.Floor(transform.position.x);
        currentTileY = (int)Math.Floor(transform.position.y);
        fogTilemap.ClearAllTiles();
        for (int x = currentTileX - 2; x <= currentTileX + 2; x++)
        {
            for (int y = currentTileY - 2; y <= currentTileY + 2; y++)
            {
                if (x == currentTileX && y == currentTileY)
                {
                    fogTilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
                else
                {
                    fogTilemap.SetTile(new Vector3Int(x, y, 0), whiteTile);
                }
            }
        }

        float horz = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        float vert = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        float horzMag = Math.Abs(horz);
        float vertMag = Math.Abs(vert);
        int facingX = 0;
        int facingY = 0;
        if (vertMag > horzMag)
        {
            if (vert >= 0)
            {
                // Facing North
                facingY++;
            }
            else
            {
                // Facing South
                facingY--;
            }
        }
        else
        {
            if (horz >= 0)
            {
                // Facing East
                facingX++;
            }
            else
            {
                // Facing West
                facingX--;
            }
        }
        fogTilemap.SetTile(new Vector3Int(currentTileX + facingX, currentTileY + facingY, 0), null);
        fogTilemap.SetTile(new Vector3Int(currentTileX + 2 * facingX, currentTileY + 2 * facingY, 0), null);
    }
}
