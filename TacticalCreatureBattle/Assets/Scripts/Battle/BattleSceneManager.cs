using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    StateMachine StateMachine { get; set; }

    GameObject _dontDestroy;

    public void Initialize()
    {
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

        // TODO: Create the battle.

        // Change to the next state.
        //StateMachine.ChangeState<>();
    }
}
