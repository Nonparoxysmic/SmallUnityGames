using UnityEngine.SceneManagement;

public class StateInitialization : State
{
    public override void Enter()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        switch (activeScene.name)
        {
            case "TitleScreen":
                StateMachine.ChangeState<TitleScreenStart>();
                break;
            case "MainMenu":
                StateMachine.ChangeState<MainMenuStart>();
                break;
            case "Battle":
                StateMachine.ChangeState<BattleStart>();
                break;
            default:
                this.Error($"No initial state set for active scene \"{activeScene.name}\".");
                break;
        }
    }

    public override void Exit()
    {
        Destroy(this);
    }
}
