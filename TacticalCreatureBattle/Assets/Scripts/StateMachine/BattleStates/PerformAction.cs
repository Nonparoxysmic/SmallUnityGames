using System.Collections;
using UnityEngine;

public class PerformAction : BattleState
{
    public override void Enter()
    {
        StartCoroutine(Battle.CurrentAction.PerformAction());
        StartCoroutine(WaitForActionComplete());
    }

    IEnumerator WaitForActionComplete()
    {
        yield return new WaitUntil(() => Battle.CurrentAction.ActionCompleted);
        Destroy(Battle.CurrentAction.gameObject);
        StateMachine.ChangeState<ActionSelection>();
    }
}
