using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitsInCells : ActionInstruction
{
    [SerializeField] ListLabel CellList;
    [SerializeField] TeamAlignment TeamToSelect = TeamAlignment.Both;
    [SerializeField] ListLabel StoreUnits;

    public override IEnumerator Execute()
    {
        List<UnitController> targets = new List<UnitController>();
        if (Action.TargetCells[(int)CellList].Count == 0)
        {
            Action.TargetUnits[(int)StoreUnits] = targets;
            yield break;
        }
        List<UnitController> unitPool = Battle.Units;
        switch (TeamToSelect)
        {
            case TeamAlignment.SameTeam:
                unitPool = Battle.ActiveUnit.Team == Team.Human ? Battle.HumanTeam : Battle.ComputerTeam;
                break;
            case TeamAlignment.OpposingTeam:
                unitPool = Battle.ActiveUnit.Team == Team.Human ? Battle.ComputerTeam : Battle.HumanTeam;
                break;
        }
        foreach (UnitController unit in unitPool)
        {
            if (!unit.InBattle)
            {
                continue;
            }
            for (int i = 0; i < (unit.UnitSize == Size.Large ? 2 : 1); i++)
            {
                for (int j = 0; j < (unit.UnitSize == Size.Large ? 2 : 1); j++)
                {
                    Vector2Int unitCell = (Vector2Int)unit.Position + new Vector2Int(i, j);
                    if (Action.TargetCells[(int)CellList].Contains(unitCell))
                    {
                        targets.Add(unit);
                        goto NestedBreak;
                    }
                }
            }
            NestedBreak:;
        }
        Action.TargetUnits[(int)StoreUnits] = targets;
    }
}
