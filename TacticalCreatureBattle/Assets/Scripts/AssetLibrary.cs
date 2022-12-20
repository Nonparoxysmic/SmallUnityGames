using UnityEngine;

public class AssetLibrary : MonoBehaviour
{
    public SpriteBook SpriteBook;

    static AssetLibrary _instance;

    void Awake()
    {
        _instance = this;
        if (_instance == null ||
            _instance.SpriteBook == null ||
            _instance.SpriteBook.CreatureSpritesSmall == null ||
            _instance.SpriteBook.CreatureSpritesMedium == null ||
            _instance.SpriteBook.CreatureSpritesLarge == null ||
            _instance.SpriteBook.CreatureSpritesSmall.Length == 0 ||
            _instance.SpriteBook.CreatureSpritesMedium.Length == 0 ||
            _instance.SpriteBook.CreatureSpritesLarge.Length == 0)
        {
            this.Error("Missing sprites in AssetLibrary SpriteBook.");
            return;
        }
    }

    public static Sprite GetSprite(Size size, uint index)
    {
        switch (size)
        {
            case Size.Small:
                index = (uint)(index % _instance.SpriteBook.CreatureSpritesSmall.Length);
                return _instance.SpriteBook.CreatureSpritesSmall[index];
            case Size.Medium:
                index = (uint)(index % _instance.SpriteBook.CreatureSpritesMedium.Length);
                return _instance.SpriteBook.CreatureSpritesMedium[index];
            case Size.Large:
                index = (uint)(index % _instance.SpriteBook.CreatureSpritesLarge.Length);
                return _instance.SpriteBook.CreatureSpritesLarge[index];
            default:
                return null;
        }
    }
}
