public class ActionCleanup : BattleState
{
    public override void Enter()
    {
        // TODO: All ungrounded, nonflying units fall.

        // TODO: If the active unit is dead or cannot do any more actions, end the turn.
        //StateMachine.ChangeState<EndOfTurn>();
        // Otherwise, return to action selection.
        StateMachine.ChangeState<ActionSelection>();
    }
}
