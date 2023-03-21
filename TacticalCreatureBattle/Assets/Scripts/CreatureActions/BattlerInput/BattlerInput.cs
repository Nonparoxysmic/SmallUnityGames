using System;
using System.Collections;
using UnityEngine;

public class BattlerInput : ActionInstruction
{
    protected bool _inputCancelled;
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
            _inputCancelled = false;
            _inputSubmitted = false;
            KeyboardInput.DirectionalInput += OnDirectionalInput;
            KeyboardInput.KeyDown += OnKeyDown;
            Battle.UI.EndBattleButtonClick += OnEndBattleButtonClick;
            yield return new WaitUntil(() => _inputSubmitted);
            KeyboardInput.DirectionalInput -= OnDirectionalInput;
            KeyboardInput.KeyDown -= OnKeyDown;
            Battle.UI.EndBattleButtonClick -= OnEndBattleButtonClick;
            if (_inputCancelled)
            {
                Action.InstructionSuccess = false;
            }
            else
            {
                Resolve();
            }
        }
        Destroy(_cursor.gameObject);
        Battle.UI.ClearCellHighlights();
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
        if (Battle.UI.IsPaused)
        {
            return;
        }
        if (e.KeyCode == KeyCode.Return || e.KeyCode == KeyCode.Space)
        {
            _inputSubmitted = true;
        }
        if (e.KeyCode == KeyCode.Backspace)
        {
            _inputCancelled = true;
            _inputSubmitted = true;
        }
    }

    protected virtual void OnEndBattleButtonClick(object sender, EventArgs e)
    {
        Action.ActionCanceled = true;
        _inputCancelled = true;
        _inputSubmitted = true;
    }
}
