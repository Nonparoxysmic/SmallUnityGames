using UnityEngine.SceneManagement;

public class BattleEnd : BattleState
{
    public override void Enter()
    {
        Battle.UI.SetButtons("Exit to Main Menu");
        Battle.UI.SetBackButtonInteractable(false);

        Battle.UI.ButtonClick += OnActionSelected;
        CameraController.AllowFreeMovement = true;
    }

    public override void Exit()
    {
        Battle.UI.ButtonClick -= OnActionSelected;
        CameraController.AllowFreeMovement = false;
    }

    private void OnActionSelected(object sender, IntegerEventArgs e)
    {
        if (e.Data == 1)
        {
            SceneManager.LoadScene("MainMenu");
            StateMachine.ChangeState<MainMenuStart>();
        }
    }
}
