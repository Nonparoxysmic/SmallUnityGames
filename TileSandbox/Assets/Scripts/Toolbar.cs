using UnityEngine;

public class Toolbar : MonoBehaviour
{
    [SerializeField] Transform cursor;

    public int numberOfOptions;

    internal void SetOption(int option)
    {
        float x = -0.5f * (numberOfOptions + 1) + option;
        cursor.localPosition = new Vector3(x, cursor.localPosition.y, cursor.localPosition.z);
    }
}
