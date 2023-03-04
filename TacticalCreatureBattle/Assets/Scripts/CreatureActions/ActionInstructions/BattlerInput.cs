using System.Collections;
using UnityEngine;

public class BattlerInput : ActionInstruction
{
    protected bool _inputSubmitted;
    protected bool _invalidInput;
    protected Transform _cursor;

    public override IEnumerator Execute()
    {
        _cursor = CreateCursor(0);
        _invalidInput = false;
        Initialize();
        if (_invalidInput)
        {
            Action.InstructionSuccess = false;
        }
        else
        {
            KeyboardInput.DirectionalInput += OnDirectionalInput;
            KeyboardInput.KeyDown += OnKeyDown;
            _inputSubmitted = false;
            yield return new WaitUntil(() => _inputSubmitted);
            KeyboardInput.DirectionalInput -= OnDirectionalInput;
            KeyboardInput.KeyDown -= OnKeyDown;
            Resolve();
        }
        Destroy(_cursor.gameObject);
    }

    Transform CreateCursor(int index)
    {
        GameObject cursor = new GameObject { name = "Cursor" };
        GameObject sprite = new GameObject { name = "Sprite" };
        sprite.transform.parent = cursor.transform;
        SpriteRenderer sr = sprite.AddComponent<SpriteRenderer>();
        sr.sprite = AssetLibrary.GetCursorSprite(index);
        sprite.transform.position += (Vector3)sr.sprite.rect.size / (2 * sr.sprite.pixelsPerUnit);
        cursor.transform.position += 9 * Vector3.back; // Camera is at -10
        return cursor.transform;
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
