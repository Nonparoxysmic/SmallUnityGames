using System;

public class ActionSelection : BattleState
{
    public override void Enter()
    {
        Battle.UI.SetButtons("Move", "Basic Attack", "Special Attack");
        if (Battle.ActiveUnit.HasMoved || Battle.ActiveUnit.MovementActionNames.Length == 0)
        {
            Battle.UI.SetButtonInteractable(1, false);
        }
        if (Battle.ActiveUnit.HasBasicAttacked || Battle.ActiveUnit.BasicActionNames.Length == 0)
        {
            Battle.UI.SetButtonInteractable(2, false);
        }
        Battle.UI.SetButtonInteractable(3, false); // Special Attacks not implemented.
        Battle.UI.SetBackButtonInteractable(false);

        // Immediately end the turn if the unit has no actions available.
        if (Battle.ActiveUnit.MovementActionNames.Length
            + Battle.ActiveUnit.BasicActionNames.Length == 0)
        {
            StateMachine.ChangeState<EndOfTurn>();
            return;
        }

        Battle.UI.ButtonClick += OnActionSelected;
        Battle.UI.TurnEnded += OnTurnEnded;
        Battle.UI.EndBattleButtonClick += OnEndBattleButtonClick;
        CameraController.AllowFreeMovement = true;
    }

    public override void Exit()
    {
        Battle.UI.ButtonClick -= OnActionSelected;
        Battle.UI.TurnEnded -= OnTurnEnded;
        Battle.UI.EndBattleButtonClick -= OnEndBattleButtonClick;
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

    private void OnEndBattleButtonClick(object sender, EventArgs e)
    {
        StateMachine.ChangeState<BattleEnd>();
    }
}
