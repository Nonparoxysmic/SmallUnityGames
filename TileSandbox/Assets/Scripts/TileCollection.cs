using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCollection : MonoBehaviour
{
    [SerializeField] Tile[] basicTiles;

    Tile noTile;

    void Start()
    {
        foreach (Tile tile in basicTiles)
        {
            if (tile == null)
            {
                this.Error("Tile reference not set in Inspector.");
            }
        }

        noTile = ScriptableObject.CreateInstance<Tile>();
        noTile.sprite = Sprite.Create
            (
                Texture2D.whiteTexture,
                new Rect(0, 0, 1, 1),
                new Vector2(0.5f, 0.5f),
                1
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
}
