public class ActionCleanup : BattleState
{
    public override void Enter()
    {
        // TODO: All ungrounded, nonflying units fall.

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
        StateMachine.ChangeState<ActionSelection>();
    }
}
