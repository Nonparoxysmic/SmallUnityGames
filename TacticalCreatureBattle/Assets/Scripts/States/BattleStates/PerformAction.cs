using System;
using System.Collections;
using UnityEngine;

public class PerformAction : BattleState
{
    bool _battleEnded;

    public override void Enter()
    {
        Battle.UI.ClearButtons();
        Battle.UI.SetBackButtonInteractable(false);
        ActionInstruction.Battle = Battle;
        StartCoroutine(Battle.CurrentAction.PerformAction());
        StartCoroutine(WaitForActionComplete());
        _battleEnded = false;
        Battle.UI.EndBattleButtonClick += OnEndBattleButtonClick;
    }

    public override void Exit()
    {
        Battle.UI.EndBattleButtonClick -= OnEndBattleButtonClick;
    }

    IEnumerator WaitForActionComplete()
    {
        yield return new WaitUntil(() => Battle.CurrentAction.ActionCompleted);
        Destroy(Battle.CurrentAction.gameObject);
        if (_battleEnded)
        {
            StateMachine.ChangeState<BattleEnd>();
        }
        else
        {
            StateMachine.ChangeState<ActionCleanup>();
        }
    }

    private void OnEndBattleButtonClick(object sender, EventArgs e)
    {
        _battleEnded = true;
    }
}
