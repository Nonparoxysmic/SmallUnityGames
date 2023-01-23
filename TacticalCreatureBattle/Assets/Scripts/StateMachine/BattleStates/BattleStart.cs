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
        // TODO: Load the correct level scene.
        //SceneManager.LoadScene("LevelName", LoadSceneMode.Additive);

        // Wait until the next frame so the level will be loaded.
        yield return null;

        // Get the level collision reference.
        GridWalls gridWalls = FindObjectOfType<GridWalls>();
        if (gridWalls == null)
        {
            this.Error("Unable to find level's collision information.");
            yield break;
        }

        // Create Battle with level collision.
        Battle = new Battle(gridWalls);

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
