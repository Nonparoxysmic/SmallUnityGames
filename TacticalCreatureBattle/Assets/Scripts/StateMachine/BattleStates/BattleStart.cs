using UnityEngine;

public class BattleStart : BattleState
{
    public override void Enter()
    {
        Battle = new Battle();

        // TODO: Load level scene.

        // Create creature units.
        Transform unitParent = new GameObject { name = "Units" }.transform;
        foreach (CreatureStats creature in Menagerie.HumanTeam)
        {
            Menagerie.CreateUnit(creature, Battle, Team.Human, unitParent);
        }
        foreach (CreatureStats enemy in Menagerie.ComputerTeam)
        {
            Menagerie.CreateUnit(enemy, Battle, Team.Computer, unitParent);
        }
        // Initialize position of units.
        float unitX = 0.5f;
        foreach (UnitController unit in Battle.Units)
        {
            unit.gameObject.transform.position = new Vector3(unitX, 0.5f);
            if (unit.UnitSize == Size.Large)
            {
                unitX += 2;
            }
            else
            {
                unitX++;
            }
        }

        // TODO: Change to next state.
        //StateMachine.ChangeState<>();
    }

    public override void Exit()
    {
        Destroy(this);
    }
}
