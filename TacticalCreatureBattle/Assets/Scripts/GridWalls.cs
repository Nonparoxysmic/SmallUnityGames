using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class GridWalls : MonoBehaviour
{
    [HideInInspector] public Color WallColor;
    [HideInInspector] public Color FallStopColor;
    [HideInInspector] public float WallThickness;
    [HideInInspector] public GridWallsTool CurrentTool;

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

            // TODO: convert cell positions to world positions

            Handles.DrawLine(cellStart, cellEnd, WallThickness);
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
        // TODO: Implement this.

        Debug.LogError("NotImplemented");
    }

    void DrawFallStopLineSegment(ulong lineSegment)
    {
        // TODO: Implement this.

        Debug.LogError("NotImplemented");
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
}
