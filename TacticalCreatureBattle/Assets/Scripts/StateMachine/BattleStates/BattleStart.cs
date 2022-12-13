using UnityEngine;

public class BattleStart : BattleState
{
    public override void Enter()
    {

    }

    public override void Exit()
    {
        Destroy(this);
    }
}
