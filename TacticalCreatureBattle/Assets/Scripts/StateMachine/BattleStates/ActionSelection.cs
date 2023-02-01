using System;
using UnityEngine;

public class ActionSelection : BattleState
{
    public override void Enter()
    {
        Battle.UI.SetButtons("Example Action 1", "Example Action 2");
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
                Debug.Log("Example Action 1 selected.");
                //StateMachine.ChangeState<>();
                break;
            case 2:
                Debug.Log("Example Action 2 selected.");
                //StateMachine.ChangeState<>();
                break;
        }
    }

    void OnTurnEnded(object sender, EventArgs e)
    {
        StateMachine.ChangeState<EndOfTurn>();
    }
}
