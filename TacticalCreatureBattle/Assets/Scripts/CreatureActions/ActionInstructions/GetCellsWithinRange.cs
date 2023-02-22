using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GetCellsWithinRange : ActionInstruction
{
    [SerializeField] SourceType Source;
    [SerializeField] ListLabel SourceList;
    [SerializeField] ValueSource RangeSource = ValueSource.Value;
    public int Range;
    public bool IncludeSource;
    [SerializeField] ListLabel StoreResult;

    public override IEnumerator Execute()
    {
        Vector2Int[] coords;
        int rangeValue = Range;
        if (RangeSource != ValueSource.Value)
        {
            rangeValue = Action.Registers[(int)RangeSource];
        }
        switch (Source)
        {
            case SourceType.TargetUnit:
                if (Action.TargetUnits[(int)SourceList].Count == 0)
                {
                    coords = Array.Empty<Vector2Int>();
                    break;
                }
                UnitController targetUnit = Action.TargetUnits[(int)SourceList][0];
                coords = Battle.Pathfinder.GetCellsWithinRange(targetUnit, rangeValue, IncludeSource);
                break;
            case SourceType.TargetCell:
                if (Action.TargetCells[(int)SourceList].Count == 0)
                {
                    coords = Array.Empty<Vector2Int>();
                    break;
                }
                Vector2Int target = Action.TargetCells[(int)SourceList][0];
                coords = Battle.Pathfinder.GetCellsWithinRange(target.x, target.y, Size.Medium, rangeValue, IncludeSource);
                break;
            default:
                coords = Battle.Pathfinder.GetCellsWithinRange(Battle.ActiveUnit, rangeValue, IncludeSource);
                break;
        }
        Action.TargetCells[(int)StoreResult] = coords.ToList();
        yield break;
    }

    enum SourceType
    {
        ActiveUnit,
        TargetUnit,
        TargetCell
    }
}
