using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public List<UnitController> Units = new List<UnitController>();
    public List<UnitController> ComputerTeam = new List<UnitController>();
    public List<UnitController> HumanTeam = new List<UnitController>();

    CreatureAction _currentAction;
    public CreatureAction CurrentAction
    {
        get
        {
            return _currentAction;
        }
        set
        {
            ActionInstruction.Battle = this;
            _currentAction = value;
        }
    }

    public UnitController ActiveUnit { get; set; }
    public TurnOrder TurnOrder { get; }
    public BattleUI UI { get; }

    readonly GridWalls _gridWalls;

    public Battle(GridWalls gridWalls, BattleUI ui)
    {
        _gridWalls = gridWalls;
        TurnOrder = new TurnOrder(ref Units);
        UI = ui;
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
