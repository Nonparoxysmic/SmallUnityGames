using System.Linq;

public class EndOfTurn : BattleState
{
    public override void Enter()
    {
        // End the battle if one or both teams have been eliminated.
        int computerTeamRemaining = Battle.ComputerTeam.Count(u => u.InBattle);
        int humanTeamRemaining = Battle.HumanTeam.Count(u => u.InBattle);
        if (computerTeamRemaining == 0 && humanTeamRemaining == 0)
        {
            Battle.Winner = BattleWinner.Tie;
        }
        else if (computerTeamRemaining == 0)
        {
            Battle.Winner = BattleWinner.Human;
        }
        else if (humanTeamRemaining == 0)
        {
            Battle.Winner = BattleWinner.Computer;
        }
        if (Battle.Winner != BattleWinner.TBD)
        {
            // Exit the battle loop.
            StateMachine.ChangeState<BattleEnd>();
            return;
        }

        // Start the next turn.
        StateMachine.ChangeState<StartOfTurn>();
    }
}
