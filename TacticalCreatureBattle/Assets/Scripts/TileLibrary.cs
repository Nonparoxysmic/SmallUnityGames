using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLibrary : MonoBehaviour
{
    public static TileLibrary Instance { get; private set; }

    public Sprite[] DevelopmentTileSprites;

    static Tile[] _developmentTiles;

    void Awake()
    {
        Instance = this;
        if (DevelopmentTileSprites.Length != 5)
        {
            this.Error($"Invalid {nameof(DevelopmentTileSprites)} array.");
            return;
        }
        foreach (Sprite sprite in DevelopmentTileSprites)
        {
            if (sprite == null)
            {
                this.Error($"Missing sprite reference in {nameof(DevelopmentTileSprites)} array.");
                return;
            }
            if (sprite.texture.width != 100 || sprite.texture.height != 100)
            {
                this.Error($"Invalid sprite dimensions in {nameof(DevelopmentTileSprites)} array.");
                return;
            }
        }
        _developmentTiles = CreateDevelopmentTiles();
    }

    static Tile[] CreateDevelopmentTiles()
    {
        Tile[] output = new Tile[32];
        for (int i = 0; i < 32; i++)
        {
            // Read collision flags of index.
            bool bottom = (i & 0b0001) > 0;
            bool right_side = (i & 0b0010) > 0;
            bool left_side = (i & 0b0100) > 0;
            bool top_solid = (i & 0b1000) > 0;
            bool top_drop_allowed = (i & 0b0001_0000) > 0 && top_solid;
            // Make list of development textures to combine.
            List<Texture2D> textures = new List<Texture2D>();
            if (bottom)
            {
                textures.Add(Instance.DevelopmentTileSprites[0].texture);
            }
            if (right_side)
            {
                textures.Add(Instance.DevelopmentTileSprites[1].texture);
            }
            if (left_side)
            {
                textures.Add(Instance.DevelopmentTileSprites[2].texture);
            }
            if (top_drop_allowed)
            {
                textures.Add(Instance.DevelopmentTileSprites[3].texture);
            }
            else if (top_solid)
            {
                textures.Add(Instance.DevelopmentTileSprites[4].texture);
            }
            // Create development tile.
            output[i] = CreateTile(CombineTextures(textures, 100, 100));
        }
        return output;
    }

    /// <summary>
    /// Combines multiple 2D textures into a single 2D texture.
    /// </summary>
    /// <remarks>
    /// All textures to be combined must have the same dimensions.
    /// Textures earlier in the list are considered to be "under" textures later in the list.
    /// </remarks>
    /// <param name="textures">An collection of 2D textures, each the same size.</param>
    /// <param name="textureWidth">The width of each texture.</param>
    /// <param name="textureHeight">The height of each texture.</param>
    static Texture2D CombineTextures(List<Texture2D> textures, int textureWidth, int textureHeight)
    {
        if (textures.Count == 0)
        {
            return new Texture2D(textureWidth, textureHeight);
        }
        Texture2D outputTexture = CopyTexture2D(textures[0]);
        if (textures.Count == 1)
        {
            return outputTexture;
        }
        for (int i = 1; i < textures.Count; i++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                for (int y = 0; y < textureHeight; y++)
                {
                    Color backgroundColor = outputTexture.GetPixel(x, y);
                    Color foregroundColor = textures[i].GetPixel(x, y);

                    // Alpha compositing.
                    float outputAlpha = foregroundColor.a + backgroundColor.a * (1 - foregroundColor.a);
                    Color outputColor = foregroundColor * foregroundColor.a;
                    outputColor += backgroundColor * backgroundColor.a * (1 - foregroundColor.a);
                    outputColor /= outputAlpha;
                    outputColor.a = outputAlpha;

                    outputTexture.SetPixel(x, y, outputColor);
                }
            }
            outputTexture.Apply();
        }
        return outputTexture;
    }

    static Texture2D CopyTexture2D(Texture2D texture)
    {
        Texture2D output = new Texture2D(texture.width, texture.height);
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                output.SetPixel(x, y, texture.GetPixel(x, y));
            }
        }
        return output;
    }

    static Tile CreateTile(Texture2D texture)
    {
        Tile output = ScriptableObject.CreateInstance<Tile>();
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        output.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
        output.flags = TileFlags.None;
        return output;
    }

    public static (Tile, Color) GetTile(uint data)
    {
        // TODO: Add more tiles.
        return (GetDevelopmentTile(data), GetColor(data));
    }

    static Color GetColor(uint data)
    {
        float r = (data >> 6 & 0b0011) / 3.0f;
        float g = (data >> 4 & 0b0011) / 3.0f;
        float b = (data >> 2 & 0b0011) / 3.0f;
        float a = (data & 0b0011) / 3.0f;
        return new Color(r, g, b, a);
    }

    static Tile GetDevelopmentTile(uint data)
    {
        uint index = data >> 8 & 0b0001_1111;
        return _developmentTiles[index];
    }
}
