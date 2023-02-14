using System;
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

        int[,] distances = new int[_levelBoundary.width, _levelBoundary.height];
        distances.Fill(int.MaxValue);

        // TODO: Finish this.
        throw new NotImplementedException();
    }
}
