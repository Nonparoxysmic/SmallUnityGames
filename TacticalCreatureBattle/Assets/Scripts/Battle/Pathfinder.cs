using UnityEngine;

public class Pathfinder
{
    readonly GridWalls _gridWalls;
    readonly Rect _levelBoundary;

    public Pathfinder(GridWalls gridWalls)
    {
        _gridWalls = gridWalls;
        _levelBoundary = _gridWalls.GetBoundaries(4);
        if (_levelBoundary == Rect.zero)
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
}
