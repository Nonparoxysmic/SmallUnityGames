using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager Instance { get; private set; }

    public CreatureData CreatureData { get; private set; }
    public World World { get; private set; }

    StateMachine StateMachine { get; set; }

    GameObject _dontDestroy;
    WorldLoader _worldLoader;

    public void Initialize()
    {
        // Create and verify all of the component references used by the battle.
        Instance = this;
        _dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        if (_dontDestroy == null)
        {
            this.Error("Cannot find \"DontDestroy\" GameObject.");
            return;
        }
        StateMachine = _dontDestroy.GetComponent<StateMachine>();
        if (StateMachine == null)
        {
            this.Error($"Missing or unavailable {typeof(StateMachine)} component.");
            return;
        }
        _worldLoader = _dontDestroy.GetComponent<WorldLoader>();
        if (_worldLoader == null)
        {
            this.Error($"Missing or unavailable {typeof(WorldLoader)} component.");
            return;
        }
        CreatureData = _dontDestroy.GetComponent<CreatureData>();
        if (CreatureData == null)
        {
            this.Error($"Missing or unavailable {typeof(CreatureData)} component.");
            return;
        }

        // Create the battle.
        World = gameObject.AddComponent<World>();
        World.CreateWorld(_worldLoader);
        // TODO: Use creature data from CreatureData.
        // TODO: Finish creating the battle.

        // TODO: Change to the next state after initialization is complete.
        //StateMachine.ChangeState<>();
    }
}
