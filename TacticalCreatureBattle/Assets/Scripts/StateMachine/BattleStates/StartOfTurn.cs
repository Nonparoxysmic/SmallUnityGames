public class StartOfTurn : BattleState
{
    public override void Enter()
    {
        // Determine the active unit for this turn.
        Battle.ActiveUnit = Battle.TurnOrder.NextUnit();

        // The active unit uses its initiative for the turn.
        Battle.ActiveUnit.ConsumeInitiative();

        // Reset the action flags.
        Battle.ActiveUnit.HasMoved = false;
        Battle.ActiveUnit.HasBasicAttacked = false;

        // Move the camera to the active unit.
        CameraController.LookAtUnit(Battle.ActiveUnit);

        // Change to the next state.
        StateMachine.ChangeState<ActionSelection>();
    }
}
