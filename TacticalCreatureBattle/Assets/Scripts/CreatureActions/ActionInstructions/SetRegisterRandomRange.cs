using System.Collections;
using UnityEngine;

public class SetRegisterRandomRange : ActionInstruction
{
    [SerializeField] ValueSource MinimumSource = ValueSource.Value;
    public int MinimumValue;
    [SerializeField] ValueSource MaximumSource = ValueSource.Value;
    public int MaximumValue;
    [SerializeField] RegisterLabel StoreResult;

    public override IEnumerator Execute()
    {
        int min = MinimumValue;
        if (MinimumSource != ValueSource.Value)
        {
            min = Action.Registers[(int)MinimumSource];
        }
        int max = MaximumValue;
        if (MaximumSource != ValueSource.Value)
        {
            max = Action.Registers[(int)MaximumSource];
        }
		if (max < min)
        {
			max = min;
		}
        Action.Registers[(int)StoreResult] = Random.Range(min, max + 1);
        yield break;
    }
}
