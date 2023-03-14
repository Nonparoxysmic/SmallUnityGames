using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStart : BattleState
{
    public override void Enter()
    {
        StartCoroutine(InitializeBattle());
    }

    IEnumerator InitializeBattle()
    {
        // Load the level scene.
        SceneManager.LoadScene("ExampleLevel", LoadSceneMode.Additive);

        // Wait until the next frame so the level will be loaded.
        yield return null;

        // Get the level collision reference.
        GridWalls gridWalls = FindObjectOfType<GridWalls>();
        if (gridWalls == null)
        {
            this.Error("Unable to find level's collision information.");
            yield break;
        }

        // Get the battle UI controller.
        BattleUI battleUI = FindObjectOfType<BattleUI>();
        if (battleUI == null)
        {
            this.Error("Unable to find battle UI.");
            yield break;
        }

        // Create Battle with level collision and UI.
        Battle = new Battle(gridWalls, battleUI);

        // Create creature units.
        Transform unitParent = new GameObject { name = "Units" }.transform;
        foreach (CreatureStats creature in Menagerie.HumanTeam)
        {
            Menagerie.CreateUnit(creature, Battle, Team.Human, unitParent);
        }
        foreach (CreatureStats enemy in Menagerie.ComputerTeam)
        {
            Menagerie.CreateUnit(enemy, Battle, Team.Computer, unitParent);
        }

        // Change to the state for unit staging.
        StateMachine.ChangeState<Staging>();
    }

    public override void Exit()
    {
        Destroy(this);
    }
}
