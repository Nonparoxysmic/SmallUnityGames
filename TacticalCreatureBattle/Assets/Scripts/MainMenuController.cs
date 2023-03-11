using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static StateMachine StateMachine { get; set; }

    public GameObject MainPanel;

    static MainMenuController _instance;

    void OnEnable()
    {
        _instance = this;
        if (MainPanel == null)
        {
            this.Error($"{nameof(MainPanel)} reference not set in the Inspector.");
            return;
        }
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

    public static void ShowMainPanel(bool doShow)
    {
        _instance.MainPanel.SetActive(doShow);
    }
}
