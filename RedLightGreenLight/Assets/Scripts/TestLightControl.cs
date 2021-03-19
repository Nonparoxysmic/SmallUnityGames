using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestLightControl : MonoBehaviour
{
    [SerializeField] Tilemap lightTilemap;
    [SerializeField] Tile whiteTile;
    SpriteRenderer lightStatusRenderer;
    [SerializeField] SpriteRenderer bgRenderer;
    bool lightsOn;

    void Awake()
    {
        lightStatusRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(LightToggleLoop());
    }

    void ToggleLight()
    {
        Tile tileToSet = null;
        if (!lightsOn) tileToSet = whiteTile;
        Vector3Int tilePos;
        for (int x = -9; x <= 9; x++)
        {
            for (int y = -9; y <= 9; y++)
            {
                tilePos = new Vector3Int(x, y, 0);
                lightTilemap.SetTile(tilePos, tileToSet);
            }
        }
        lightsOn = !lightsOn;
        if (lightsOn)
        {
            lightStatusRenderer.color = Color.white;
            bgRenderer.color = Color.black;
        }
        else
        {
            lightStatusRenderer.color = Color.black;
            bgRenderer.color = Color.white;
        }
    }

    IEnumerator LightToggleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(6);
            ToggleLight();
        }
    }
}
