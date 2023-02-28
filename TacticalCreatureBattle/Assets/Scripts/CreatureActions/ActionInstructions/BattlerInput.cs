using System.Collections;
using UnityEngine;

public class BattlerInput : ActionInstruction
{
    protected bool _inputSubmitted;
    protected GameObject _cursor;

    public override IEnumerator Execute()
    {
        _cursor = new GameObject { name = "Cursor" };
        // TODO: Add sprite to cursor GameObject.
        Initialize();
        KeyboardInput.DirectionalInput += OnDirectionalInput;
        KeyboardInput.KeyDown += OnKeyDown;
        _inputSubmitted = false;
        yield return new WaitUntil(() => _inputSubmitted);
        KeyboardInput.DirectionalInput -= OnDirectionalInput;
        KeyboardInput.KeyDown -= OnKeyDown;
        Resolve();
        Destroy(_cursor);
    }

    protected virtual void Initialize() { }

    protected virtual void Resolve() { }

    protected virtual void OnDirectionalInput(object sender, DirectionEventArgs e) { }

    protected virtual void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.Return || e.KeyCode == KeyCode.Space)
        {
            _inputSubmitted = true;
        }
    }
}
