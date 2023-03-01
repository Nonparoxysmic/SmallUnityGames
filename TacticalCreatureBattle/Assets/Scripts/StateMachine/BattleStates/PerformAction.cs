using System.Collections;
using UnityEngine;

public class PerformAction : BattleState
{
    public override void Enter()
    {
        Battle.UI.ClearButtons();
        Battle.UI.SetBackButtonInteractable(false);
        ActionInstruction.Battle = Battle;
        StartCoroutine(Battle.CurrentAction.PerformAction());
        StartCoroutine(WaitForActionComplete());
    }

    IEnumerator WaitForActionComplete()
    {
        yield return new WaitUntil(() => Battle.CurrentAction.ActionCompleted);
        Destroy(Battle.CurrentAction.gameObject);
        StateMachine.ChangeState<ActionCleanup>();
    }
}
