using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class GridWalls : MonoBehaviour
{
    [HideInInspector] public Color WallColor;
    [HideInInspector] public Color FallStopColor;
    [HideInInspector] public float WallThickness;
    [HideInInspector] public GridWallsTool CurrentTool { get; set; }

    [HideInInspector] [SerializeField] List<ulong> segmentList;
    HashSet<ulong> _segmentHashSet;

    Grid _grid;

    void OnEnable()
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
        _segmentHashSet = new HashSet<ulong>();
        foreach (ulong u in segmentList)
        {
            _segmentHashSet.Add(u);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (ulong segment in _segmentHashSet)
        {
            if (IsFallStop(segment))
            {
                Handles.color = FallStopColor;
            }
            else
            {
                Handles.color = WallColor;
            }
            (Vector3 cellStart, Vector3 cellEnd) = DecodeVectors(segment);
            Vector3 localStart = _grid.CellToLocalInterpolated(cellStart);
            Vector3 localEnd = _grid.CellToLocalInterpolated(cellEnd);
            Handles.DrawLine(_grid.LocalToWorld(localStart), _grid.LocalToWorld(localEnd), WallThickness);
        }
    }

    public void Click(Vector2 worldPosition)
    {
        if (CurrentTool == GridWallsTool.None)
        {
            return;
        }
        Vector3 localPosition = _grid.WorldToLocal(worldPosition);
        Vector3 cellPosition = _grid.LocalToCellInterpolated(localPosition);
        ulong lineSegment = EncodeVectors(NearestGridLineSegment(cellPosition));
        switch (CurrentTool)
        {
            case GridWallsTool.Toggle:
                ToggleLineSegment(lineSegment);
                break;
            case GridWallsTool.Erase:
                EraseLineSegment(lineSegment);
                break;
            case GridWallsTool.Wall:
                DrawWallLineSegment(lineSegment);
                break;
            case GridWallsTool.FallStop:
                DrawFallStopLineSegment(lineSegment);
                break;
        }
    }

    void ToggleLineSegment(ulong lineSegment)
    {
        ulong fallStop = lineSegment | (1UL << 60);
        if (_segmentHashSet.Remove(lineSegment))
        {
            segmentList.Remove(lineSegment);
            segmentList.Add(fallStop);
            _segmentHashSet.Add(fallStop);
        }
        else if (_segmentHashSet.Remove(fallStop))
        {
            segmentList.Remove(fallStop);
        }
        else
        {
            segmentList.Add(lineSegment);
            _segmentHashSet.Add(lineSegment);
        }
        EditorUtility.SetDirty(this);
    }

    void EraseLineSegment(ulong lineSegment)
    {
        if (_segmentHashSet.Remove(lineSegment))
        {
            segmentList.Remove(lineSegment);
            EditorUtility.SetDirty(this);
        }
        ulong sameLine = lineSegment ^ (1UL << 60);
        if (_segmentHashSet.Remove(sameLine))
        {
            segmentList.Remove(sameLine);
            EditorUtility.SetDirty(this);
        }
    }

    void DrawWallLineSegment(ulong lineSegment)
    {
        ulong fallStop = lineSegment | (1UL << 60);
        if (_segmentHashSet.Remove(fallStop))
        {
            segmentList.Remove(fallStop);
            EditorUtility.SetDirty(this);
        }
        if (_segmentHashSet.Add(lineSegment))
        {
            segmentList.Add(lineSegment);
            EditorUtility.SetDirty(this);
        }
    }

    void DrawFallStopLineSegment(ulong lineSegment)
    {
        ulong fallStop = lineSegment | (1UL << 60);
        if (_segmentHashSet.Remove(lineSegment))
        {
            segmentList.Remove(lineSegment);
            EditorUtility.SetDirty(this);
        }
        if (_segmentHashSet.Add(fallStop))
        {
            segmentList.Add(fallStop);
            EditorUtility.SetDirty(this);
        }
    }
#endif

    /// <summary>
    /// Gets the coordinates of the grid line segment closest to the interpolated cell position.
    /// </summary>
    /// <returns>
    /// A tuple of two <seealso cref="Vector3"/>, representing the cell position 
    /// of the start of the line segment, and the cell position of the end 
    /// of the line segment, in that order.
    /// </returns>
    /// <param name="cellPosition">An interpolated cell position.</param>
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

    /// <summary>
    /// Encodes a tuple of 2D vectors into a <seealso cref="ulong"/> for serialization.
    /// </summary>
    /// <remarks>
    /// This method encodes the X and Y components of the pair of input vectors
    /// into a <seealso cref="ulong"/> for the purposes of serializing a collection
    /// of grid line segments. The X and Y components should have integer values, 
    /// as this method is intended to encode the cell position vectors produced by the 
    /// <seealso cref="NearestGridLineSegment(Vector3)"/> method.
    /// Each component of the vectors is encoded as a 14 bit integer, plus a bit flag
    /// indicating a negative value.
    /// </remarks>
    /// <returns>
    /// The pair of vectors encoded as a <seealso cref="ulong"/>.
    /// </returns>
    /// <param name="vectors">A pair of vectors with integer-value X and Y components.</param>
    ulong EncodeVectors((Vector3 start, Vector3 end) vectors)
    {
        float[] input = new float[] { vectors.start.x, vectors.start.y, vectors.end.x, vectors.end.y };
        ulong[] components = new ulong[4];
        for (int i = 0; i < 4; i++)
        {
            if (input[i] < 0)
            {
                components[i] = ((ulong)Mathf.Abs(input[i])) & 0b0011_1111_1111_1111;
                components[i] |= 0b0100_0000_0000_0000;
            }
            else
            {
                components[i] = ((ulong)input[i]) & 0b0011_1111_1111_1111;
            }
        }
        return (components[0] << 45) | (components[1] << 30) | (components[2] << 15) | components[3];
    }

    /// <summary>
    /// Gets the position of the 2D grid line segment represented by the input.
    /// </summary>
    /// <remarks>
    /// This method extracts the X and Y components of the two vectors encoded in the input.
    /// Each component of the vectors is encoded as a 14 bit integer, plus a bit flag
    /// indicating a negative value.
    /// </remarks>
    /// <returns>
    /// A tuple of 2 <seealso cref="Vector3"/>, representing the start and end
    /// cell positions of the line segment encoded in the input.
    /// </returns>
    /// <param name="input">A 2D grid line segment represented as a <seealso cref="ulong"/></param>
    (Vector3, Vector3) DecodeVectors(ulong input)
    {
        int[] shift = new int[] { 45, 30, 15, 0 };
        float[] output = new float[4];
        for (int i = 0; i < 4; i++)
        {
            output[i] = (input >> shift[i]) & 0b0011_1111_1111_1111;
            bool isNegative = ((input >> shift[i]) & 0b0100_0000_0000_0000) > 0;
            if (isNegative)
            {
                output[i] *= -1;
            }
        }
        return (new Vector3(output[0], output[1]), new Vector3(output[2], output[3]));
    }

    bool IsFallStop(ulong input)
    {
        return (input & (1UL << 60)) > 0;
    }

    public bool CanMove(Vector3Int cell, Direction direction)
    {
        return !_segmentHashSet.Contains(LineSegment(cell, direction));
    }

    public bool CanFall(Vector3Int cell, Direction direction)
    {
        ulong wall = LineSegment(cell, direction);
        ulong fallStop = wall | (1UL << 60);
        return !_segmentHashSet.Contains(wall) && !_segmentHashSet.Contains(fallStop);
    }

    ulong LineSegment(Vector3Int cell, Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return EncodeVectors((cell, cell + Vector3Int.up));
            case Direction.Down:
                return EncodeVectors((cell, cell + Vector3Int.right));
            case Direction.Right:
                return EncodeVectors((cell + Vector3Int.right, cell + Vector3Int.right + Vector3Int.up));
            case Direction.Up:
                return EncodeVectors((cell + Vector3Int.up, cell + Vector3Int.up + Vector3Int.right));
            default:
                this.Error("Invalid Direction parameter.");
                return 0;
        }
    }

    public RectInt GetBoundaries(int buffer)
    {
        if (buffer < 1)
        {
            buffer = 1;
        }
        int xMax = int.MinValue, yMax = int.MinValue;
        int xMin = int.MaxValue, yMin = int.MaxValue;
        foreach (ulong segment in _segmentHashSet)
        {
            (Vector3 start, Vector3 end) = DecodeVectors(segment);
            xMin = (int)Mathf.Min(xMin, start.x);
            yMin = (int)Mathf.Min(yMin, start.y);
            xMax = (int)Mathf.Max(xMax, end.x);
            yMax = (int)Mathf.Max(yMax, end.y);
        }
        if (xMax < xMin || yMax < yMin)
        {
            return new RectInt(0, 0, 0, 0);
        }
        return new RectInt(xMin - buffer, yMin - buffer, xMax - xMin + 2 * buffer - 1, yMax - yMin + 2 * buffer - 1);
    }
}
