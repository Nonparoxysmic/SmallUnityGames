using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    readonly GridWalls _gridWalls;
    readonly RectInt _levelBoundary;

    public Pathfinder(GridWalls gridWalls)
    {
        _gridWalls = gridWalls;
        _levelBoundary = _gridWalls.GetBoundaries(4);
        if (_levelBoundary.width == 0 || _levelBoundary.height == 0)
        {
            Debug.LogError($"{nameof(Pathfinder)}: Level boundary is undefined.");
        }
    }

    public bool CanMove(Vector3Int cell, Direction direction)
    {
        return _gridWalls.CanMove(cell, direction);
    }

    public bool CanFall(Vector3Int cell, Direction direction)
    {
        return _gridWalls.CanFall(cell, direction);
    }

    public Vector2Int[] GetCellsWithinRange(UnitController unit, int range, bool includeUnitSpace)
    {
        int x = (int)unit.transform.position.x;
        int y = (int)unit.transform.position.y;
        return GetCellsWithinRange(x, y, unit.UnitSize, range, includeUnitSpace);
    }

    public Vector2Int[] GetCellsWithinRange(int x, int y, Size size, int range, bool includeUnitSpace)
    {
        if (range < 0 || (range == 0 && !includeUnitSpace))
        {
            return Array.Empty<Vector2Int>();
        }
        // Calculate each cell's distance from the source, up to the range.
        int[,] distances = CalculateDistances(x, y, size, range);
        // If not including the unit space, override the calculated distances.
        if (!includeUnitSpace)
        {
            for (int i = 0; i < (size == Size.Large ? 2 : 1); i++)
            {
                for (int j = 0; j < (size == Size.Large ? 2 : 1); j++)
                {
                    distances[x - _levelBoundary.xMin + i, y - _levelBoundary.yMin + j] = int.MaxValue;
                }
            }
        }
        // Return the collection of cell coordinates in range.
        List<Vector2Int> coords = new List<Vector2Int>();
        for (int i = 0; i < distances.GetLength(0); i++)
        {
            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (distances[i, j] <= range)
                {
                    coords.Add(new Vector2Int(i + _levelBoundary.xMin, j + _levelBoundary.yMin));
                }
            }
        }
        return coords.ToArray();
    }

    int[,] CalculateDistances(int x, int y, Size size, int maxRange)
    {
        int[,] distances = new int[_levelBoundary.width, _levelBoundary.height];
        distances.Fill(int.MaxValue);

        // TODO: Finish this.
        throw new NotImplementedException();
    }
}
