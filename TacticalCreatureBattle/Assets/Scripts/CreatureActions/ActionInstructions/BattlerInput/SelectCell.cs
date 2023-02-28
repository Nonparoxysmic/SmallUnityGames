using System.Collections.Generic;
using UnityEngine;

public class SelectCell : BattlerInput
{
    // TODO: Add option to limit selection to list of cells.
    [SerializeField] ListLabel StoreResult;

    Vector2Int _output;

    protected override void Initialize()
    {
        _output = (Vector2Int)Battle.ActiveUnit.Position;
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
        _output += e.Direction;
        _cursor.position = new Vector3(_output.x, _output.y, _cursor.position.z);
        CameraController.LookAtCell(_output.x, _output.y);
    }

    protected override void Resolve()
    {
        Action.TargetCells[(int)StoreResult] = new List<Vector2Int>() { _output };
    }
}
