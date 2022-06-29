using System.Collections.Generic;
using UnityEngine;

public class Toolbar : MonoBehaviour
{
    public int Size { get; private set; }

    [SerializeField] Transform cursor;
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] Sprite[] toolSprites;

    [HideInInspector] public int current;

    SpriteRenderer[] iconSpriteRenderers;

    public void Create(int size)
    {
        Size = size;
        if (cursor is null)
        {
            this.Error("Toolbar cursor reference not set in Inspector.");
            return;
        }
        if (backgroundPrefab is null)
        {
            this.Error("Prefab reference not set in Inspector.");
            return;
        }
        if (iconPrefab is null)
        {
            this.Error("Prefab reference not set in Inspector.");
            return;
        }
        if (toolSprites is null || toolSprites.Length == 0)
        {
            this.Error("Tool sprites not set in Inspector.");
            return;
        }
        if (Size < 1 || Size > 9)
        {
            this.Error("Toolbar size must be in the range [1, 9].");
            return;
        }
        int toolNumber = 0;
        List<SpriteRenderer> srList = new List<SpriteRenderer>();
        for (float x = -0.5f * (Size - 1); x <= 0.5f * (Size - 1); x++)
        {
            Vector3 pos = transform.position + new Vector3(x, 0, 0);
            GameObject gameObject = Instantiate(iconPrefab, pos, Quaternion.identity, transform);
            gameObject.name = $"Toolbar Icon {toolNumber}";
            srList.Add(gameObject.GetComponent<SpriteRenderer>());
            pos += new Vector3(0, 0, 2);
            gameObject = Instantiate(backgroundPrefab, pos, Quaternion.identity, transform);
            gameObject.name = $"Toolbar Background {toolNumber}";
            toolNumber++;
        }
        iconSpriteRenderers = srList.ToArray();
        for (int i = 0; i < toolSprites.Length; i++)
        {
            SetIcon(i, toolSprites[i]);
        }
        SetCurrent(0);
    }

    public void SetCurrent(int option)
    {
        if (!option.InRange(0, Size)) { return; }

        current = option;
        float x = -0.5f * (Size - 1) + current;
        cursor.localPosition = new Vector3(x, cursor.localPosition.y, cursor.localPosition.z);
    }

    void SetIcon(int index, Sprite sprite)
    {
        if (!index.InRange(0, iconSpriteRenderers.Length)) { return; }
        if (iconSpriteRenderers[index] == null)
        {
            this.Error("Missing or unavailable Sprite Renderer.");
        }
        iconSpriteRenderers[index].sprite = sprite;
    }
}
