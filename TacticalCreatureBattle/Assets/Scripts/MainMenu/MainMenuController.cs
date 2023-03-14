using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static StateMachine StateMachine { get; set; }

    public GameObject MainPanel;
    public GameObject ChooseTeamsPanel;

    static MainMenuController _instance;

    void OnEnable()
    {
        _instance = this;
        if (MainPanel == null)
        {
            this.Error($"{nameof(MainPanel)} reference not set in the Inspector.");
            return;
        }
        if (ChooseTeamsPanel == null)
        {
            this.Error($"{nameof(ChooseTeamsPanel)} reference not set in the Inspector.");
            return;
        }
        ShowMainPanel(false);
        ShowTeamPanel(false);
    }

    public static void StartBattle()
    {
        SceneManager.LoadScene("Battle");
        StateMachine.ChangeState<BattleStart>();
    }

    public static void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public static void ReturnFromSubmenu()
    {
        StateMachine.ChangeState<MainSelection>();
    }

    public static void OnChooseTeamsButton()
    {
        StateMachine.ChangeState<TeamSelection>();
    }

    public static void ShowMainPanel(bool doShow)
    {
        _instance.MainPanel.SetActive(doShow);
    }

    public static void ShowTeamPanel(bool doShow)
    {
        _instance.ChooseTeamsPanel.SetActive(doShow);
    }
}
