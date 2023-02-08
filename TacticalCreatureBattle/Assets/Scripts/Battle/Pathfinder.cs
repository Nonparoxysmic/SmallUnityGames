using UnityEngine;

public class Pathfinder
{
    readonly GridWalls _gridWalls;

    public Pathfinder(GridWalls gridWalls)
    {
        _gridWalls = gridWalls;
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
