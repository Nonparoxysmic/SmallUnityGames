using System.Linq;

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
            while (Battle.Pathfinder.UnitCanFall(unit, Direction.Down))
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

        // Determine how many opponents remain.
        int enemyTeamRemaining;
        if (Battle.ActiveUnit.Team == Team.Computer)
        {
            enemyTeamRemaining = Battle.HumanTeam.Count(u => u.InBattle);
        }
        else
        {
            enemyTeamRemaining = Battle.ComputerTeam.Count(u => u.InBattle);
        }

        // If the active unit is knocked out or cannot do any more actions, 
        // or the opposing team has been eliminated, or the turn was ended
        // during the action, go to the end of turn state.
        if (!Battle.ActiveUnit.InBattle ||
            (Battle.ActiveUnit.HasMoved && Battle.ActiveUnit.HasBasicAttacked)
            || enemyTeamRemaining == 0
            || Battle.CurrentAction.TurnEnded)
        {
            Battle.CurrentAction.TurnEnded = false;
            StateMachine.ChangeState<EndOfTurn>();
        }
        // Otherwise, return to action selection.
        else
        {
            StateMachine.ChangeState<ActionSelection>();
        }
    }
}
