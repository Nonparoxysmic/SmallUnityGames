using System.Collections;
using UnityEngine;

public class SetRegister : ActionInstruction
{
    [SerializeField] ValueSource Source;
    public int Value;
    [SerializeField] RegisterLabel StoreResult;

    public override IEnumerator Execute()
    {
        int val = Value;
        if (Source != ValueSource.Value)
        {
            val = Action.Registers[(int)Source];
        }
        Action.Registers[(int)StoreResult] = val;
        yield break;
    }
}
