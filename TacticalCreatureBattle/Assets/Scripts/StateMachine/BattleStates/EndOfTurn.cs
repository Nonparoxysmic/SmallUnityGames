public class EndOfTurn : BattleState
{
    public override void Enter()
    {
        // TODO: Exit the battle loop if end conditions are met.

        // Start the next turn.
        StateMachine.ChangeState<StartOfTurn>();
    }
}
