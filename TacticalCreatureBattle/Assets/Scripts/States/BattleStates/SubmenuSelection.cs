using System;
using System.Collections.Generic;

public abstract class SubmenuSelection : BattleState
{
    public abstract string[] GetUnitActionNames();

    string[] actionNames;
    string[] displayNames;

    public override void Enter()
    {
        actionNames = GetActionNames();
        displayNames = Menagerie.GetActionDisplayNames(actionNames);
        Battle.UI.SetButtons(displayNames[0], displayNames[1], displayNames[2], displayNames[3]);
        Battle.UI.SetBackButtonInteractable(true);

        Battle.UI.BackButtonClick += OnBackButton;
        Battle.UI.ButtonClick += OnActionSelected;
        Battle.UI.TurnEnded += OnTurnEnded;
        Battle.UI.EndBattleButtonClick += OnEndBattleButtonClick;
        CameraController.AllowFreeMovement = true;
    }

    string[] GetActionNames()
    {
        string[] unitActionNames = GetUnitActionNames();
        List<string> output = new List<string>();
        for (int i = 0; i < Math.Min(unitActionNames.Length, 4); i++)
        {
            output.Add(unitActionNames[i]);
        }
        for (int i = unitActionNames.Length; i < 4; i++)
        {
            output.Add("");
        }
        return output.ToArray();
    }

    public override void Exit()
    {
        Battle.UI.BackButtonClick -= OnBackButton;
        Battle.UI.ButtonClick -= OnActionSelected;
        Battle.UI.TurnEnded -= OnTurnEnded;
        Battle.UI.EndBattleButtonClick -= OnEndBattleButtonClick;
        CameraController.AllowFreeMovement = false;
    }

    void OnActionSelected(object sender, IntegerEventArgs e)
    {
        if (e.Data < 1 || e.Data > 4)
        {
            return;
        }
        if (actionNames[e.Data - 1] != "")
        {
            if (Menagerie.TryGetAction(actionNames[e.Data - 1], out CreatureAction action))
            {
                Battle.CurrentAction = Instantiate(action);
                StateMachine.ChangeState<PerformAction>();
            }
        }
    }

    void OnTurnEnded(object sender, EventArgs e)
    {
        StateMachine.ChangeState<EndOfTurn>();
    }

    private void OnBackButton(object sender, EventArgs e)
    {
        StateMachine.ChangeState<ActionSelection>();
    }

    private void OnEndBattleButtonClick(object sender, EventArgs e)
    {
        StateMachine.ChangeState<BattleEnd>();
    }
}

public class MovementSelection : SubmenuSelection
{
    public override string[] GetUnitActionNames()
    {
        return Battle.ActiveUnit.MovementActionNames;
    }
}

public class BasicAttackSelection : SubmenuSelection
{
    public override string[] GetUnitActionNames()
    {
        return Battle.ActiveUnit.BasicActionNames;
    }
}

public class SpecialAttackSelection : SubmenuSelection
{
    public override string[] GetUnitActionNames()
    {
        return Battle.ActiveUnit.SpecialActionNames;
    }
}
