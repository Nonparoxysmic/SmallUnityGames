using System.Collections;
using UnityEngine;

public class PerformAction : BattleState
{
    public override void Enter()
    {
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
