using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCollection : MonoBehaviour
{
    [SerializeField] Tile[] basicTiles;
    [SerializeField] Sprite holeGradientSprite;

    readonly Dictionary<int, Tile> holeTiles = new Dictionary<int, Tile>();
    Tile noTile;

    void Start()
    {
        foreach (Tile tile in basicTiles)
        {
            if (tile is null)
            {
                this.Error("Tile reference not set in Inspector.");
            }
        }
        if (holeGradientSprite is null)
        {
            this.Error("Hole gradient sprite not set in Inspector.");
        }

        noTile = ScriptableObject.CreateInstance<Tile>();
        noTile.sprite = Sprite.Create
            (
                Texture2D.whiteTexture,
                new Rect(0, 0, 4, 4),
                new Vector2(0.5f, 0.5f),
                4
            );
        noTile.color = Color.magenta;
    }

    public Tile GetTile(int index)
    {
        if (index.InRange(0, basicTiles.Length))
        {
            return basicTiles[index];
        }
        return noTile;
    }

    public Tile GetHoleTile(int index)
    {
        if (holeTiles.ContainsKey(index))
        {
            return holeTiles[index];
        }
        else
        {
            Tile holeTile = CreateHoleTile(index);
            holeTiles.Add(index, holeTile);
            return holeTile;
        }
    }

    private Tile CreateHoleTile(int index)
    {
        Tile baseTile = GetTile(index);
        Texture2D combinedTexture = CombineSpriteTextures(baseTile.sprite, holeGradientSprite);
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile.sprite = Sprite.Create
            (
                combinedTexture,
                new Rect(0, 0, 128, 128),
                new Vector2(0.5f, 0.5f),
                128
            );
        newTile.colliderType = Tile.ColliderType.Sprite;
        return newTile;
    }

    private Texture2D CombineSpriteTextures(Sprite baseSprite, Sprite overlaySprite)
    {
        Texture2D baseTx = baseSprite.texture;
        Texture2D overlayTx = overlaySprite.texture;

        if (!128.EqualsAll(baseTx.width, baseTx.height, overlayTx.width, overlayTx.height))
        {
            this.Error(nameof(CombineSpriteTextures) + ": Invalid sprite dimensions.");
            return baseSprite.texture;
        }

        Texture2D outputTx = new Texture2D(128, 128);
        for (int y = 0; y < 128; y++)
        {
            for (int x = 0; x < 128; x++)
            {
                Color baseColor = baseTx.GetPixel(x, y);
                Color overlayColor = overlayTx.GetPixel(x, y);

                float outputAlpha = overlayColor.a + baseColor.a * (1 - overlayColor.a);
                Color outputColor = overlayColor * overlayColor.a;
                outputColor += baseColor * baseColor.a * (1 - overlayColor.a);
                outputColor /= outputAlpha;
                outputColor.a = outputAlpha;

                outputTx.SetPixel(x, y, outputColor);
            }
        }
        outputTx.Apply();
        return outputTx;
    }
}
