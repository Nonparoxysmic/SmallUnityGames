using UnityEngine;

[ExecuteAlways]
public class SpawnPoint : MonoBehaviour
{
    public Team Team;
    public uint Size;
    [HideInInspector] public Vector3Int Cell;

    Grid _grid;

    void Awake()
    {
        ValidateGrid();
    }

    void OnEnable()
    {
        ValidateGrid();
    }

    private void OnTransformParentChanged()
    {
        ValidateGrid();
    }

    void ValidateGrid()
    {
        if (transform.parent == null || transform.parent.name != "Spawn Points")
        {
            this.Error($"{typeof(SpawnPoint)} must be child of \"Spawn Points\" GameObject.");
            return;
        }
        if (transform.parent.parent == null)
        {
            this.Error($"\"Spawn Points\" GameObject requires parent Grid.");
            return;
        }
        _grid = transform.parent.parent.GetComponent<Grid>();
        if (_grid == null)
        {
            this.Error($"\"Spawn Points\" GameObject requires parent Grid.");
            return;
        }
        if (_grid.cellLayout != GridLayout.CellLayout.Rectangle)
        {
            this.Error($"{typeof(SpawnPoint)} only supports Rectangle grids.");
            return;
        }
    }

    void OnDrawGizmos()
    {
        if (_grid == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.25f);
            return;
        }
        Cell = _grid.WorldToCell(transform.position);
        transform.position = _grid.GetCellCenterWorld(Cell);
        if (Size == 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
            return;
        }
        Gizmos.color = Team == Team.Human ? Color.green : Color.blue;
        if (Size > 1)
        {
            Vector3 end = _grid.GetCellCenterWorld(Cell + new Vector3Int((int)Size - 1, 0, 0));
            Gizmos.DrawLine(transform.position, end);
        }
        for (int i = 0; i < Size; i++)
        {
            Vector3 pos = _grid.GetCellCenterWorld(Cell + new Vector3Int(i, 0, 0));
            Gizmos.DrawSphere(pos, 0.25f);
        }
    }
}
