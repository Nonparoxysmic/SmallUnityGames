using UnityEngine;

public class AssetLibrary : MonoBehaviour
{
    public SpriteBook SpriteBook;

    static AssetLibrary _instance;

    void Awake()
    {
        _instance = this;
    }

    public static Sprite GetSprite(uint index)
    {
        if (_instance == null ||
            _instance.SpriteBook == null ||
            _instance.SpriteBook.Sprites == null ||
            _instance.SpriteBook.Sprites.Length == 0)
        {
            return null;
        }
        return _instance.SpriteBook.Sprites[index % _instance.SpriteBook.Sprites.Length];
    }
}
