using System.Collections;
using UnityEngine;

public class GetUnitStat : ActionInstruction
{
    [SerializeField] UnitSource Target;
    [SerializeField] ListLabel UnitList;
    [SerializeField] StatsAndValues StatToGet;
    [SerializeField] RegisterLabel StoreResult;

    public override IEnumerator Execute()
    {
        if (Target == UnitSource.TargetUnit && Action.TargetUnits[(int)UnitList].Count == 0)
        {
            Action.Registers[(int)StoreResult] = 0;
            yield break;
        }
        UnitController unit = Target == UnitSource.ActiveUnit ? Battle.ActiveUnit : Action.TargetUnits[(int)UnitList][0];
        Action.Registers[(int)StoreResult] = StatToGet switch
        {
            StatsAndValues.Health => unit.CreatureStats.GetStatTotal(Stat.Health),
            StatsAndValues.Strength => unit.CreatureStats.GetStatTotal(Stat.Strength),
            StatsAndValues.Magic => unit.CreatureStats.GetStatTotal(Stat.Magic),
            StatsAndValues.Defense => unit.CreatureStats.GetStatTotal(Stat.Defense),
            StatsAndValues.Speed => unit.CreatureStats.GetStatTotal(Stat.Speed),
            StatsAndValues.CurrentHP => unit.CurrentHP,
            StatsAndValues.CurrentInitiative => unit.CurrentInitiative,
            StatsAndValues.HasMoved => unit.HasMoved ? 1 : 0,
            StatsAndValues.HasBasicAttacked => unit.HasBasicAttacked ? 1 : 0,
            StatsAndValues.MaximumHP => unit.CreatureStats.MaximumHP,
            StatsAndValues.Size => (int)unit.UnitSize,
            _ => 0,
        };
    }
}
