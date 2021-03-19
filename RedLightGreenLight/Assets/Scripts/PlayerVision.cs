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
    [SerializeField] Tilemap lightTilemap;
    [SerializeField] Tilemap wallTilemap;
    Vector3Int fogArrayTilePos;
    Vector3Int playerTilePos;

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
        double angle = Math.Atan2(mouseDirection.y, mouseDirection.x) / Math.PI;
        int facing = 1;
        if (angle > 0.75 || angle < -0.75) facing = 3;
        else if (angle <= -0.25) facing = 2;
        else if (angle >= 0.25) facing = 0;

        playerTilePos = playerMovement.currentTilePos;
        fogArrayTilePos = playerTilePos + new Vector3Int(-7, -7, 0);
        for (int x = 0; x < 15; x++)
        {
            for (int y = 0; y < 15; y++)
            {
                Vector3Int lookTilePos = fogArrayTilePos + new Vector3Int(x, y, 0);
                if (fogTilemap.GetTile(lookTilePos) == null && wallTilemap.GetTile(lookTilePos) == null)
                {
                    if (lightTilemap.GetTile(lookTilePos) == null)
                    {
                        fogTilemap.SetTile(lookTilePos, blackTile);
                    }
                    else
                    {
                        fogTilemap.SetTile(lookTilePos, whiteTile);
                    }
                }
            }
        }
        for (int x = 0; x < 15; x++)
        {
            for (int y = 0; y < 15; y++)
            {
                Vector3Int lookTilePos = fogArrayTilePos + new Vector3Int(x, y, 0);
                if (x == 7 && y == 7)
                {
                    fogTilemap.SetTile(lookTilePos, null);
                    continue;
                }

                int lookTileLightRequired = 1;
                if (lightTilemap.GetTile(lookTilePos) == null)
                {
                    lookTileLightRequired = 2;
                }
                if (visionPatterns[facing][lookTilePos.x - fogArrayTilePos.x, lookTilePos.y - fogArrayTilePos.y] < lookTileLightRequired)
                {
                    continue;
                }

                Vector3 ray = lookTilePos - playerTilePos;
                Vector3 playerPos = playerTilePos + new Vector3(0.5f, 0.5f, 0);
                int steps = (int)Math.Max(Math.Abs(ray.x), Math.Abs(ray.y));
                Vector3Int stepTilePos;
                for (int i = 1; i <= steps; i++)
                {
                    Vector3 stepPos = playerPos + ray * i / steps;
                    stepTilePos = new Vector3Int((int)Math.Floor(stepPos.x), (int)Math.Floor(stepPos.y), 0);
                    int stepTileLightRequired = 1;
                    if (lightTilemap.GetTile(stepTilePos) == null)
                    {
                        stepTileLightRequired = 2;
                    }
                    if (visionPatterns[facing][stepTilePos.x - fogArrayTilePos.x, stepTilePos.y - fogArrayTilePos.y] >= stepTileLightRequired)
                    {
                        fogTilemap.SetTile(stepTilePos, null);
                    }
                    if (wallTilemap.GetTile(stepTilePos) != null)
                    {
                        break;
                    }
                }
            }
        }
    }
}
