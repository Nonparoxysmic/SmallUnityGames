using UnityEngine;

public abstract class State : MonoBehaviour
{
    public StateMachine StateMachine { get; set; }

    public virtual void Enter() { }

    public virtual void Exit() { }
}
