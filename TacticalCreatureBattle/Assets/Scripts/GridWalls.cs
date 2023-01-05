using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class GridWalls : MonoBehaviour
{
    [HideInInspector] public Color WallColor;
    [HideInInspector] public float WallThickness;

    [SerializeField] List<ulong> segmentList;
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

    void OnDrawGizmosSelected()
    {
        Handles.color = WallColor;
        foreach (ulong segment in _segmentHashSet)
        {
            (Vector3 start, Vector3 end) = DecodeVectors(segment);

            // TODO: convert cell positons to world positions

            Handles.DrawLine(start, end, WallThickness);
        }
    }

    public void Click(Vector2 worldPosition)
    {
        Vector3 localPosition = _grid.WorldToLocal(worldPosition);
        Vector3 cellPosition = _grid.LocalToCellInterpolated(localPosition);
        ulong lineSegment = EncodeVectors(NearestGridLineSegment(cellPosition));
        if (_segmentHashSet.Add(lineSegment))
        {
            segmentList.Add(lineSegment);
        }
        else if (_segmentHashSet.Remove(lineSegment))
        {
            segmentList.Remove(lineSegment);
        }
        EditorUtility.SetDirty(this);
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

    ulong EncodeVectors((Vector3 start, Vector3 end) vectors)
    {
        float[] input = new float[] { vectors.start.x, vectors.start.y, vectors.end.x, vectors.end.y };
        ulong[] components = new ulong[4];
        for (int i = 0; i < 4; i++)
        {
            if (input[i] < 0)
            {
                components[i] = ((ulong)Mathf.Abs(input[i])) & 0b0111_1111_1111_1111;
                components[i] |= 0b1000_0000_0000_0000;
            }
            else
            {
                components[i] = ((ulong)input[i]) & 0b0111_1111_1111_1111;
            }
        }
        return (components[0] << 48) | (components[1] << 32) | (components[2] << 16) | components[3];
    }

    (Vector3, Vector3) DecodeVectors(ulong input)
    {
        int[] shift = new int[] { 48, 32, 16, 0 };
        float[] output = new float[4];
        for (int i = 0; i < 4; i++)
        {
            output[i] = (input >> shift[i]) & 0b0111_1111_1111_1111;
            bool isNegative = ((input >> shift[i]) & 0b1000_0000_0000_0000) > 0;
            if (isNegative)
            {
                output[i] *= -1;
            }
        }
        return (new Vector3(output[0], output[1]), new Vector3(output[2], output[3]));
    }
}
