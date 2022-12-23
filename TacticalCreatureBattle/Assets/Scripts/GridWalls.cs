using UnityEngine;

public class GridWalls : MonoBehaviour
{
    public Color WallColor { get; set; }

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

    public void OnDrawGizmosSelected()
    {

    }

    public void Click(Vector2 worldPosition)
    {

    }
}
