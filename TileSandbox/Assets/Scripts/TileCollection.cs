using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCollection : MonoBehaviour
{
    public string WaterTileName { get; private set; }

    [SerializeField] Tile[] basicTiles;
    [SerializeField] Sprite holeGradientSprite;

    readonly Dictionary<int, Tile> holeTiles = new Dictionary<int, Tile>();
    Tile holeBaseTile;
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

        WaterTileName = basicTiles[0].name;

        noTile = ScriptableObject.CreateInstance<Tile>();
        noTile.sprite = Utilities.BlankSquareSprite(128, Color.magenta);

        holeBaseTile = ScriptableObject.CreateInstance<Tile>();
        holeBaseTile.sprite = Utilities.BlankSquareSprite(4, Color.black);
    }

    public int GetTileIndex(string name)
    {
        for (int i = 0; i < basicTiles.Length; i++)
        {
            Tile tile = basicTiles[i];
            if (tile.name == name)
            {
                return i;
            }
        }
        return -1;
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
        // Use dirt instead of grass.
        if (index == 2) { index = 3; }

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

    public Tile HoleBase()
    {
        return holeBaseTile;
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
        outputTx.wrapMode = TextureWrapMode.Clamp;
        return outputTx;
    }
}
