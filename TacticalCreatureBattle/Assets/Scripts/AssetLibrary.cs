using UnityEngine;

public class AssetLibrary : MonoBehaviour
{
    [SerializeField] Sprite[] CursorSprites;

    static AssetLibrary _instance;

    void OnEnable()
    {
        _instance = this;
        if (CursorSprites == null || CursorSprites.Length == 0)
        {
            this.Error("No cursor sprites set in Inspector.");
            return;
        }
        foreach (Sprite s in CursorSprites)
        {
            if (s == null)
            {
                this.Error("Missing cursor sprite reference in Inspector.");
                return;
            }
        }
    }

    public static Sprite GetCursorSprite(int index)
    {
        if (index < 0)
        {
            return _instance.CursorSprites[0];
        }
        return _instance.CursorSprites[index % _instance.CursorSprites.Length];
    }
}
