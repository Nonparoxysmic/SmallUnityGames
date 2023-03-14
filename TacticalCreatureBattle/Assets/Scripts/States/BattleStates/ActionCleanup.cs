public class ActionCleanup : BattleState
{
    public override void Enter()
    {
        // All ungrounded, nonflying units fall.
        foreach (UnitController unit in Battle.Units)
        {
            if (!unit.InBattle || unit.CreatureStats.Species.CanFly)
            {
                continue;
            }
            while (Battle.Pathfinder.CanFall(unit.Position, Direction.Down))
            {
                unit.MoveTo(unit.Position + UnityEngine.Vector3Int.down);
                if (unit.Position.y < Battle.Pathfinder.MinimumY)
                {
                    unit.RemoveFromBattle();
                    break;
                }
            }
        }

        // All knocked-out units are removed from the battle.
        foreach (UnitController unit in Battle.Units)
        {
            if (unit.InBattle && unit.CurrentHP <= 0)
            {
                unit.RemoveFromBattle();
            }
        }

        // If the active unit is knocked out or cannot do any more actions, end the turn.
        if (!Battle.ActiveUnit.InBattle ||
            (Battle.ActiveUnit.HasMoved && Battle.ActiveUnit.HasBasicAttacked))
        {
            StateMachine.ChangeState<EndOfTurn>();
        }
        // Otherwise, return to action selection.
        else
        {
            StateMachine.ChangeState<ActionSelection>();
        }
    }
}
