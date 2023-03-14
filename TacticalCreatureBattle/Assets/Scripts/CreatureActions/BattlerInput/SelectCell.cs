using System.Collections.Generic;
using UnityEngine;

public class SelectCell : BattlerInput
{
    public bool SelectFromList;
    [SerializeField] ListLabel SelectFrom;
    [SerializeField] ListLabel StoreResult;

    Vector2Int _output;

    protected override void Initialize()
    {
        if (SelectFromList && Action.TargetCells[(int)SelectFrom].Count == 0)
        {
            _invalidInput = true;
            return;
        }
        if (SelectFromList && !Action.TargetCells[(int)SelectFrom].Contains((Vector2Int)Battle.ActiveUnit.Position))
        {
            _output = Action.TargetCells[(int)SelectFrom][0];
        }
        else
        {
            _output = (Vector2Int)Battle.ActiveUnit.Position;
        }
        _cursor.position = new Vector3(_output.x, _output.y, _cursor.position.z);
        CameraController.LookAtCell(_output.x, _output.y);
    }

    protected override void OnDirectionalInput(object sender, DirectionEventArgs e)
    {
        // Only allow valid movement.
        Direction direction = Direction.Left;
        if (e.Direction.y < 0)
        {
            direction = Direction.Down;
        }
        else if (e.Direction.x > 0)
        {
            direction = Direction.Right;
        }
        else if (e.Direction.y > 0)
        {
            direction = Direction.Up;
        }
        if (!Battle.Pathfinder.CanMove((Vector3Int)_output, direction))
        {
            return;
        }
        // Update the selection.
        if (SelectFromList)
        {
            Vector2Int neighbor = _output += e.Direction;
            if (Action.TargetCells[(int)SelectFrom].Contains(neighbor))
            {
                _output = neighbor;
            }
            else
            {
                int index = Action.TargetCells[(int)SelectFrom].IndexOf(_output);
                index = index < 0 ? 0 : index + 1;
                _output = Action.TargetCells[(int)SelectFrom][index % Action.TargetCells.Length];
            }
        }
        else
        {
            _output += e.Direction;
        }
        _cursor.position = new Vector3(_output.x, _output.y, _cursor.position.z);
        CameraController.LookAtCell(_output.x, _output.y);
    }

    protected override void Resolve()
    {
        Action.TargetCells[(int)StoreResult] = new List<Vector2Int>() { _output };
    }
}
