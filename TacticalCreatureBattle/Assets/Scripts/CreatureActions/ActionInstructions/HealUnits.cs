using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUnits : ActionInstruction
{
    [SerializeField] UnitSource Target;
    [SerializeField] ListLabel UnitList;
    [SerializeField] ValueSource HealingMinAmountSource = ValueSource.Value;
    public int HealingMinAmount;
    [SerializeField] ValueSource HealingMaxAmountSource = ValueSource.Value;
    public int HealingMaxAmount;
    public bool IsPercentage;
    [SerializeField] ElementSource Element;
    [SerializeField] Element FixedElement;

    public override IEnumerator Execute()
    {
        List<UnitController> targets = Target == UnitSource.ActiveUnit ?
            new List<UnitController>() { Battle.ActiveUnit } : Action.TargetUnits[(int)UnitList];
        Element healingElement = FixedElement;
        switch (Element)
        {
            case ElementSource.ActiveUnitPrimary:
                healingElement = Battle.ActiveUnit.PrimaryElement;
                break;
            case ElementSource.ActiveUnitSecondary:
                healingElement = Battle.ActiveUnit.SecondaryElement;
                break;
        }
        int healingMinAmount = HealingMinAmount;
        if (HealingMinAmountSource != ValueSource.Value)
        {
            healingMinAmount = Action.Registers[(int)HealingMinAmountSource];
        }
        int healingMaxAmount = HealingMaxAmount;
        if (HealingMaxAmountSource != ValueSource.Value)
        {
            healingMaxAmount = Action.Registers[(int)HealingMaxAmountSource];
        }
        foreach (UnitController target in targets)
        {
            target.Heal(healingElement, Random.Range(healingMinAmount, healingMaxAmount + 1), IsPercentage);
        }
        yield break;
    }
}
