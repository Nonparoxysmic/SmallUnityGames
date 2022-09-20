using UnityEngine.SceneManagement;

public class StateInitialization : State
{
    public override void Enter()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        switch (activeScene.name)
        {
            case "TitleScreen":
                //StateMachine.ChangeState<>();
                break;
            case "MainMenu":
                //StateMachine.ChangeState<>();
                break;
            case "Battle":
                //StateMachine.ChangeState<>();
                break;
            default:
                this.Error($"No initial state set for active scene \"{activeScene.name}\".");
                break;
        }
    }
}
