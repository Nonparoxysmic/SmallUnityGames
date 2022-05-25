using UnityEngine;

public class Toolbar : MonoBehaviour
{
    public int Size { get; private set; }

    [SerializeField] Transform cursor;
    [SerializeField] GameObject squarePrefab;

    public int current;

    public void Create(int size)
    {
        Size = size;
        if (cursor is null)
        {
            this.Error("Toolbar cursor reference not set in Inspector.");
            return;
        }
        if (squarePrefab is null)
        {
            this.Error("Prefab reference not set in Inspector.");
            return;
        }
        if (Size < 1 || Size > 9)
        {
            this.Error("Toolbar size must be in the range [1, 9].");
            return;
        }
        for (float x = -0.5f * (Size - 1); x <= 0.5f * (Size - 1); x++)
        {
            Vector3 pos = transform.position + new Vector3(x, 0, 0);
            Instantiate(squarePrefab, pos, Quaternion.identity, transform);
        }
        SetCurrent(0);
    }

    public void SetCurrent(int option)
    {
        if (option < 0 || option >= Size) { return; }

        current = option;
        float x = -0.5f * (Size - 1) + current;
        cursor.localPosition = new Vector3(x, cursor.localPosition.y, cursor.localPosition.z);
    }
}
