using System.Collections;
using UnityEngine;

public class BattleStart : State
{
    [SerializeField] GameObject BattlePrefab;

    public override void Enter()
    {
        if (BattlePrefab == null)
        {
            this.Error("Prefab reference not set in the inspector.");
            return;
        }
        StartCoroutine(CreateBattle());
    }

    IEnumerator CreateBattle()
    {
        // Do nothing for the rest of the current frame.
        yield return null;

        // Create the battle.
        GameObject battle = Instantiate(BattlePrefab);
        battle.name = BattlePrefab.name;
        BattleSceneManager battleSceneManager = battle.GetComponent<BattleSceneManager>();
        if (battleSceneManager == null)
        {
            this.Error($"Missing or unavailable {typeof(BattleSceneManager)} component.");
            yield break;
        }
        battleSceneManager.Initialize();
    }

    public override void Exit()
    {
        Destroy(this);
    }
}
