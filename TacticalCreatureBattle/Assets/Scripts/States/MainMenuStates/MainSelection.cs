public class MainSelection : State
{
    public override void Enter()
    {
        MainMenuController.ShowMainPanel(true);
    }

    public override void Exit()
    {
        MainMenuController.ShowMainPanel(false);
    }
}
