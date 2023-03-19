using UnityEngine;

public class AssetLibrary : MonoBehaviour
{
    [SerializeField] Sprite[] CursorSprites;
    [SerializeField] Sprite IconTeamC;
    [SerializeField] Sprite IconTeamH;

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
        if (IconTeamC == null || IconTeamH == null)
        {
            this.Error("Missing team icon reference in Inspector.");
            return;
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

    public static Sprite GetTeamIcon(Team team)
    {
        return team == Team.Computer ? _instance.IconTeamC : _instance.IconTeamH;
    }

    public static Sprite CreateSquareSprite(int size, Color color)
    {
        size = Mathf.Max(2, Mathf.Abs(size));

        Texture2D texture = new Texture2D(size, size);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();

        Sprite sprite = Sprite.Create
            (
                texture,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f),
                size
            );
        return sprite;
    }
}
