using UnityEngine;
using UnityEditor;

public class GridWalls : MonoBehaviour
{
    [HideInInspector] public Color WallColor;
    [HideInInspector] public float WallThickness;

    Grid _grid;

    void Awake()
    {
        if (transform.parent == null)
        {
            this.Error($"{typeof(GridWalls)} requires parent Grid.");
            return;
        }
        _grid = transform.parent.GetComponent<Grid>();
        if (_grid == null)
        {
            this.Error($"{typeof(GridWalls)} requires parent Grid.");
            return;
        }
        if (_grid.cellLayout != GridLayout.CellLayout.Rectangle)
        {
            this.Error($"{typeof(GridWalls)} only supports Rectangle grids.");
            return;
        }
    }

    void OnDrawGizmosSelected()
    {

    }

    public void Click(Vector2 worldPosition)
    {

    }
}
