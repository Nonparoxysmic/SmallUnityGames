﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerVision : MonoBehaviour
{
    const float BlinkerSpeed = 180;

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

    [SerializeField] Tile blackTile;
    [SerializeField] Tile grayTile;
    [SerializeField] Tile whiteTile;
    [SerializeField] Tilemap fogTilemap;
    [SerializeField] Tilemap lightTilemap;
    [SerializeField] Tilemap wallTilemap;
    [SerializeField] Tilemap wallSidesTilemap;
    public int startDimTime;
    public int maxBlinkTime;

    [HideInInspector] public bool isFacingNorth;
    [HideInInspector] public bool isBlinking;

    GameObject blinker;
    BlinkMeter blinkMeter;
    Camera cameraComponent;
    CollisionMonitor collisionMonitor;
    SpriteRenderer dimmer;
    GameClock gameClock;
    Animator playerAnimator;
    PlayerMovement playerMovement;

    int blinkCountdown;
    KeyCode[] blinkKeyCodes;
    bool blinkPressedOffPlayerFrame;
    float blinkerTargetPosY;
    bool doBlinking;
    Vector3Int fogArrayTilePos;
    Vector3Int playerTilePos;
    Vector3Int wallSideOffset;

    void Awake()
    {
        blinker = GameObject.Find("Blinker");
        blinkMeter = GameObject.Find("Blink Meter").GetComponent<BlinkMeter>();
        cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        collisionMonitor = GameObject.Find("CollisionMonitor").GetComponent<CollisionMonitor>();
        dimmer = GameObject.Find("Dimmer").GetComponent<SpriteRenderer>();
        gameClock = GameObject.Find("Game Clock").GetComponent<GameClock>();
        playerAnimator = GameObject.Find("PlayerSprite").GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        if (startDimTime > maxBlinkTime)
        {
            startDimTime = maxBlinkTime;
        }
        wallSideOffset = new Vector3Int(0, 1, 0);
        blinkKeyCodes = InitializeBlinkKeys();
    }

    void Start()
    {
        doBlinking = true;
        gameClock.onPlayerTick.AddListener(PlayerUpdate);
        gameClock.onNonPlayerTick.AddListener(NonPlayerUpdate);
        blinkerTargetPosY = 18;
        blinkCountdown = maxBlinkTime;
        InitializeFog();
    }

    void Update()
    {
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        double angle = Math.Atan2(mouseDirection.y, mouseDirection.x) / Math.PI;
        int facing = 1;
        if (angle > 0.75 || angle < -0.75) facing = 3;
        else if (angle <= -0.25) facing = 2;
        else if (angle >= 0.25) facing = 0;
        playerAnimator.SetInteger("Direction", facing);
        if (facing == 0) isFacingNorth = true;
        else isFacingNorth = false;

        playerTilePos = playerMovement.currentTilePos;
        fogArrayTilePos = playerTilePos + new Vector3Int(-7, -7, 0);
        for (int x = -1; x < 17; x++)
        {
            for (int y = -1; y < 17; y++)
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
                        // If wall side, clear wall above also:
                        if (wallSidesTilemap.GetTile(stepTilePos) != null && y < 14)
                        {
                            fogTilemap.SetTile(stepTilePos + wallSideOffset, null);
                        }
                    }
                    if (wallTilemap.GetTile(stepTilePos) != null)
                    {
                        break;
                    }
                }
            }
        }

        if (!doBlinking) return;

        blinkMeter.SetHorzScale(Math.Max(10.0f * (blinkCountdown + startDimTime - maxBlinkTime) / startDimTime, 0));
        if (blinkCountdown < maxBlinkTime - startDimTime - 1)
        {
            float alpha;
            if (blinkCountdown < (maxBlinkTime - startDimTime) * 2.0f / 3)
            {
                alpha = 1 - blinkCountdown / (2.0f * (maxBlinkTime - startDimTime));
            }
            else
            {
                alpha = 2.0f * (blinkCountdown - maxBlinkTime + startDimTime) / (startDimTime - maxBlinkTime);
            }
            dimmer.color = new Color(dimmer.color.r, dimmer.color.g, dimmer.color.b, alpha);
        }
        else dimmer.color = new Color(dimmer.color.r, dimmer.color.g, dimmer.color.b, 0);
        if (blinker.transform.localPosition.y != blinkerTargetPosY)
        {
            float difference = blinkerTargetPosY - blinker.transform.localPosition.y;
            float step = Math.Sign(difference) * BlinkerSpeed * Time.deltaTime;
            if (Math.Abs(step) < Math.Abs(difference))
            {
                blinker.transform.localPosition = new Vector3(blinker.transform.localPosition.x, blinker.transform.localPosition.y + step, blinker.transform.localPosition.z);
            }
            else
            {
                blinker.transform.localPosition = new Vector3(blinker.transform.localPosition.x, blinkerTargetPosY, blinker.transform.localPosition.z);
            }
        }
    }

    public void PlayerUpdate()
    {
        if (!doBlinking) return;

        blinker.transform.localPosition = new Vector3(blinker.transform.localPosition.x, blinkerTargetPosY, blinker.transform.localPosition.z);
        if (blinkCountdown > 0)
        {
            blinkCountdown--;
        }
        if (blinkCountdown == 0 || ((blinkCountdown < maxBlinkTime - 2) && (blinkPressedOffPlayerFrame || BlinkPressedThisFrame())))
        {
            StartCoroutine(StartBlink());
        }
        blinkPressedOffPlayerFrame = false;
    }

    void NonPlayerUpdate()
    {
        if (!doBlinking) return;

        blinker.transform.localPosition = new Vector3(blinker.transform.localPosition.x, blinkerTargetPosY, blinker.transform.localPosition.z);
        if (blinkCountdown > 0)
        {
            blinkCountdown--;
        }
        if (blinkerTargetPosY == 0)
        {
            StartCoroutine(StopBlink());
        }
        if (BlinkPressedThisFrame() && !isBlinking)
        {
            blinkPressedOffPlayerFrame = true;
        }
    }

    bool BlinkPressedThisFrame()
    {
        foreach (KeyCode kc in blinkKeyCodes)
        {
            if (Input.GetKey(kc)) return true;
        }
        return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "Target-typed new() expression not available in Unity 2019")]
    KeyCode[] InitializeBlinkKeys()
    {
        List<KeyCode> keys = new List<KeyCode>();
        foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
        {
            if ((int)kc > 324) break;
            keys.Add(kc);
        }
        keys.Remove(KeyCode.UpArrow);
        keys.Remove(KeyCode.DownArrow);
        keys.Remove(KeyCode.RightArrow);
        keys.Remove(KeyCode.LeftArrow);
        keys.Remove(KeyCode.W);
        keys.Remove(KeyCode.A);
        keys.Remove(KeyCode.S);
        keys.Remove(KeyCode.D);
        keys.Remove(KeyCode.Escape);
        keys.Remove(KeyCode.None);
        return keys.ToArray();
    }

    void InitializeFog()
    {
        for (int x = collisionMonitor.levelBoundary.xMin - 7; x < collisionMonitor.levelBoundary.xMax + 7; x++)
        {
            for (int y = collisionMonitor.levelBoundary.yMin - 7; y < collisionMonitor.levelBoundary.yMax + 7; y++)
            {
                fogTilemap.SetTile(new Vector3Int(x, y, 0), grayTile);
            }
        }
    }

    public void SetVision(bool enabled)
    {
        if (enabled)
        {
            cameraComponent.cullingMask = -1;
        }
        else
        {
            cameraComponent.cullingMask = 1 << 5;
        }
    }

    public void TerminateBlinking()
    {
        doBlinking = false;
        Destroy(blinker);
        Destroy(blinkMeter.gameObject);
        Destroy(dimmer.gameObject);
    }

    IEnumerator StartBlink()
    {
        yield return null;
        isBlinking = true;
        blinkerTargetPosY = 0;
    }

    IEnumerator StopBlink()
    {
        yield return null;
        isBlinking = false;
        blinkerTargetPosY = 18;
        blinkCountdown = maxBlinkTime - 1;
    }
}
