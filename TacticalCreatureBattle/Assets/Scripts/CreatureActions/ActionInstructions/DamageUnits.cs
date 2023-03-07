using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUnits : ActionInstruction
{
    [SerializeField] UnitSource Target;
    [SerializeField] ListLabel UnitList;
    [SerializeField] ValueSource DamageMinAmountSource = ValueSource.Value;
    public int DamageMinAmount;
    [SerializeField] ValueSource DamageMaxAmountSource = ValueSource.Value;
    public int DamageMaxAmount;
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
        int damageMinAmount = DamageMinAmount;
        if (DamageMinAmountSource != ValueSource.Value)
        {
            damageMinAmount = Action.Registers[(int)DamageMinAmountSource];
        }
        int damageMaxAmount = DamageMaxAmount;
        if (DamageMaxAmountSource != ValueSource.Value)
        {
            damageMaxAmount = Action.Registers[(int)DamageMaxAmountSource];
        }
        foreach (UnitController target in targets)
        {
            target.TakeDamage(damageElement, Random.Range(damageMinAmount, damageMaxAmount + 1), IsPercentage);
        }
        yield break;
    }
}
