using System.Collections;

public class MainMenuStart : State
{
    public override void Enter()
    {
        StartCoroutine(InitializeMenu());
    }

    IEnumerator InitializeMenu()
    {
        MainMenuController.StateMachine = StateMachine;

        // Wait until the next frame for the scene to finish loading.
        yield return null;

        StateMachine.ChangeState<MainSelection>();
    }
}
