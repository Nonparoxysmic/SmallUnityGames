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

    /// <summary>
    /// Gets the coordinates of the grid line segment closest to the cell position.
    /// </summary>
    /// <returns>
    /// A tuple of two <seealso cref="Vector3"/>, representing the cell position 
    /// of the start of the line segment, and the cell position of the end 
    /// of the line segment, in that order.
    /// </returns>
    /// <param name="cellPosition">A cell position.</param>
    /// <returns></returns>
    (Vector3, Vector3) NearestGridLineSegment(Vector3 cellPosition)
    {
        float roundedX = Mathf.Round(cellPosition.x);
        float roundedY = Mathf.Round(cellPosition.y);
        if (Mathf.Abs(cellPosition.x - roundedX) < Mathf.Abs(cellPosition.y - roundedY))
        {
            return
                (
                    new Vector3(roundedX, Mathf.Floor(cellPosition.y), 0),
                    new Vector3(roundedX, Mathf.Floor(cellPosition.y) + 1, 0)
                );
        }
        return
            (
                new Vector3(Mathf.Floor(cellPosition.x), roundedY, 0),
                new Vector3(Mathf.Floor(cellPosition.x) + 1, roundedY, 0)
            );
    }
}
