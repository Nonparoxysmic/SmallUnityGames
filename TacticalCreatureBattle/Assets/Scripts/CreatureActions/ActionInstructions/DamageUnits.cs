using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUnits : ActionInstruction
{
    [SerializeField] UnitSource Target;
    [SerializeField] ListLabel UnitList;
    [SerializeField] ValueSource DamageAmountSource = ValueSource.Value;
    public int DamageAmount;
    public bool IsPercentage;
    [SerializeField] ElementSource Element;
    [SerializeField] Element FixedElement;

    public override IEnumerator Execute()
    {
        List<UnitController> targets = Target == UnitSource.ActiveUnit ?
            new List<UnitController>() { Battle.ActiveUnit } : Action.TargetUnits[(int)UnitList];
        Element damageElement = FixedElement;
        switch (Element)
        {
            case ElementSource.ActiveUnitPrimary:
                damageElement = Battle.ActiveUnit.PrimaryElement;
                break;
            case ElementSource.ActiveUnitSecondary:
                damageElement = Battle.ActiveUnit.SecondaryElement;
                break;
        }
        int damageAmount = DamageAmount;
        if (DamageAmountSource != ValueSource.Value)
        {
            damageAmount = Action.Registers[(int)DamageAmountSource];
        }
        foreach (UnitController target in targets)
        {
            target.TakeDamage(damageElement, damageAmount, IsPercentage);
        }
        yield break;
    }
}
