using System;

public class ActionSelection : BattleState
{
    public override void Enter()
    {
        Battle.UI.SetButtons("Move", "Basic Attack", "Special Attack");
        // TODO: If an action is unavailable, use Battle.UI.SetButtonInteractable({buttonNumber}, false).
        Battle.UI.SetBackButtonInteractable(false);

        Battle.UI.ButtonClick += OnActionSelected;
        Battle.UI.TurnEnded += OnTurnEnded;
        CameraController.AllowFreeMovement = true;
    }

    public override void Exit()
    {
        Battle.UI.ButtonClick -= OnActionSelected;
        Battle.UI.TurnEnded -= OnTurnEnded;
        CameraController.AllowFreeMovement = false;
    }

    void OnActionSelected(object sender, IntegerEventArgs e)
    {
        switch (e.Data)
        {
            case 1:
                StateMachine.ChangeState<MovementSelection>();
                break;
            case 2:
                StateMachine.ChangeState<BasicAttackSelection>();
                break;
            case 3:
                StateMachine.ChangeState<SpecialAttackSelection>();
                break;
        }
    }

    void OnTurnEnded(object sender, EventArgs e)
    {
        StateMachine.ChangeState<EndOfTurn>();
    }
}
