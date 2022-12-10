using UnityEngine;

public class StateMachine : MonoBehaviour
{
    State _currentState;

    public State CurrentState
    {
        get
        {
            return _currentState;
        }
        private set
        {
            if (value == _currentState)
            {
                Debug.LogWarning("Attempted to change to the current state.");
                return;
            }
            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _currentState = value;
            if (_currentState != null)
            {
                _currentState.Enter();
            }
            else
            {
                Debug.LogError("Current state has been set to null.");
            }
        }
    }

    public void ChangeState<T>() where T : State
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
            component.StateMachine = this;
        }
        CurrentState = component;
    }
}
