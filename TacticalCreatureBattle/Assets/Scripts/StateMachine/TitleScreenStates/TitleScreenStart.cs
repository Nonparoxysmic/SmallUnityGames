using System;
using UnityEngine.SceneManagement;

public class TitleScreenStart : State
{
    public override void Enter()
    {
        KeyboardInput.AnyKeyDown += OnAnyKeyDown;
    }

    void OnAnyKeyDown(object sender, EventArgs e)
    {
        KeyboardInput.AnyKeyDown -= OnAnyKeyDown;
        SceneManager.LoadScene("MainMenu");
        StateMachine.ChangeState<MainMenuStart>();
    }

    public override void Exit()
    {
        Destroy(this);
    }
}
