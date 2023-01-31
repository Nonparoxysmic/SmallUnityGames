using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public List<UnitController> Units = new List<UnitController>();
    public List<UnitController> ComputerTeam = new List<UnitController>();
    public List<UnitController> HumanTeam = new List<UnitController>();

    public UnitController ActiveUnit { get; set; }
    public TurnOrder TurnOrder { get; }

    readonly GridWalls _gridWalls;

    public Battle(GridWalls gridWalls)
    {
        _gridWalls = gridWalls;
        TurnOrder = new TurnOrder(ref Units);
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
