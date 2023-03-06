using System.Collections;
using UnityEngine;

public class TryMoveUnit : ActionInstruction
{
    [SerializeField] UnitSource MoveUnit;
    [SerializeField] ListLabel UnitList;
    [SerializeField] ListLabel CellList;

    public override IEnumerator Execute()
    {
        UnitController unit = Battle.ActiveUnit;
        if (MoveUnit == UnitSource.TargetUnit)
        {
            if (Action.TargetUnits[(int)UnitList].Count == 0)
            {
                MoveSuccess(false);
                yield break;
            }
            unit = Action.TargetUnits[(int)UnitList][0];
        }
        if (Action.TargetCells[(int)CellList].Count == 0)
        {
            MoveSuccess(false);
            yield break;
        }
        Vector2Int dest = Action.TargetCells[(int)CellList][0];
        if (unit.UnitSize == Size.Large && !Battle.Pathfinder.ValidSpaceForLargeUnit(dest.x, dest.y))
        {
            MoveSuccess(false);
            yield break;
        }
        unit.MoveTo(dest.x, dest.y);
        MoveSuccess(true);
    }

    void MoveSuccess(bool success)
    {
        Action.InstructionSuccess = success;
    }
}
