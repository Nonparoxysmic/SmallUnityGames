using UnityEngine;

public class BattleStart : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {
        Destroy(this);
    }
}
