public class TeamSelection : State
{
    public override void Enter()
    {
        MainMenuController.ShowTeamPanel(true);
    }

    public override void Exit()
    {
        MainMenuController.ShowTeamPanel(false);
    }
}
