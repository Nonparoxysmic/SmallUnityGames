using System.Collections.Generic;

public class Battle
{
    public List<UnitController> Units = new List<UnitController>();
    public List<UnitController> ComputerTeam = new List<UnitController>();
    public List<UnitController> HumanTeam = new List<UnitController>();

    public CreatureAction CurrentAction { get; set; }
    public UnitController ActiveUnit { get; set; }
    public Pathfinder Pathfinder { get; }
    public TurnOrder TurnOrder { get; }
    public BattleUI UI { get; }

    public Battle(GridWalls gridWalls, BattleUI ui)
    {
        Pathfinder = new Pathfinder(gridWalls);
        TurnOrder = new TurnOrder(ref Units);
        UI = ui;
    }
}
