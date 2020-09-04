using UnityEngine;

public class GridState : MonoBehaviour
{
    [SerializeField] GameObject boxPrefab;
    [HideInInspector] public Letter[,] GridLetters { get; private set; }
    LineDrawer ld;

    void Start()
    {
        if (boxPrefab == null)
        {
            boxPrefab = new GameObject("error");
        }

        GridLetters = new Letter[3, 3];
        ld = GetComponent<LineDrawer>();

        CreateBox("Box0", -2, 2);
        CreateBox("Box1", 0, 2);
        CreateBox("Box2", 2, 2);
        CreateBox("Box3", -2, 0);
        CreateBox("Box4", 0, 0);
        CreateBox("Box5", 2, 0);
        CreateBox("Box6", -2, -2);
        CreateBox("Box7", 0, -2);
        CreateBox("Box8", 2, -2);
    }

    public void UpdateGridState(int boxX, int boxY, Letter newLetter)
    {
        GridLetters[boxX, boxY] = newLetter;
        ld.DrawLines();
    }

    void CreateBox(string n, float x, float y)
    {
        GameObject newBox = Instantiate(boxPrefab);
        newBox.transform.SetParent(gameObject.transform);
        newBox.name = n;
        newBox.transform.position = new Vector2(x, y);
    }
}
