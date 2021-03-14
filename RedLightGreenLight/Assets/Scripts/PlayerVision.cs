using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerVision : MonoBehaviour
{
    readonly int[][,] visionPatterns = new int[][,]
        {
            new int[,]
                {
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2},
                    {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2}
                },
            new int[,]
                {
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1},
                    {1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1},
                    {1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                    {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2}
                },
            new int[,]
                {
                    {2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 2, 2, 3, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                    {2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0}
                },
            new int[,]
                {
                    {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                    {1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                    {1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1},
                    {1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1},
                    {1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                }
        };

    PlayerMovement playerMovement;

    [SerializeField] Tilemap fogTilemap;
    Vector2Int fogPos;

    [SerializeField] Tile blackTile;
    [SerializeField] Tile grayTile;
    [SerializeField] Tile whiteTile;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        fogTilemap.gameObject.SetActive(true);
    }

    void Update()
    {
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        double angle = Math.Atan2(mouseDirection.y, mouseDirection.x);
        int facing = 1;
        if (angle > 0.75 * Math.PI || angle < -0.75 * Math.PI)
        {
            facing = 3;
        }
        else if (angle < -0.25 * Math.PI)
        {
            facing = 2;
        }
        else if (angle > 0.25 * Math.PI)
        {
            facing = 0;
        }

        fogTilemap.ClearAllTiles();
        fogPos.x = playerMovement.currentTilePos.x - 7;
        fogPos.y = playerMovement.currentTilePos.y - 7;
        for (int x = 0; x < 15; x++)
        {
            for (int y = 0; y < 15; y++)
            {
                if (x == 7 && y == 7) continue;
                float distance = (float)Math.Sqrt(Math.Pow(x - 7, 2) + Math.Pow(y - 7, 2));
                Vector3 originPos = new Vector3(playerMovement.currentTilePos.x + 0.5f, playerMovement.currentTilePos.y + 0.5f, 0);
                RaycastHit2D hit = Physics2D.Raycast(originPos, new Vector3(x - 7, y - 7, 0), distance);
                if (hit.collider == null) continue;

                if (visionPatterns[facing][x, y] == 0)
                {
                    fogTilemap.SetTile(new Vector3Int(fogPos.x + x, fogPos.y + y, 0), blackTile);
                }
                else if (visionPatterns[facing][x, y] == 1)
                {
                    fogTilemap.SetTile(new Vector3Int(fogPos.x + x, fogPos.y + y, 0), grayTile);
                }
                else if (visionPatterns[facing][x, y] == 2)
                {
                    fogTilemap.SetTile(new Vector3Int(fogPos.x + x, fogPos.y + y, 0), whiteTile);
                }
            }
        }
    }
}
