using System;
using System.Collections;
using UnityEngine;

public class MathOp : ActionInstruction
{
    [SerializeField] ValueSource Value1Source;
    public int Value1;
    [SerializeField] MathOperation Operation;
    [SerializeField] ValueSource Value2Source;
    public int Value2;
    [SerializeField] RegisterLabel StoreResult;

    public override IEnumerator Execute()
    {
        int value1 = Value1;
        if (Value1Source != ValueSource.Value)
        {
            value1 = Action.Registers[(int)Value1Source];
        }
        int value2 = Value2;
        if (Value2Source != ValueSource.Value)
        {
            value2 = Action.Registers[(int)Value2Source];
        }
        int result;
        switch (Operation)
        {
            case MathOperation.Add:
                result = value1 + value2;
                break;
            case MathOperation.Subtract:
                result = value1 - value2;
                break;
            case MathOperation.Multiply:
                result = value1 * value2;
                break;
            case MathOperation.Divide:
                if (value2 == 0)
                {
                    yield break;
                }
                result = value1 / value2;
                break;
            case MathOperation.Modulo:
                if (value2 == 0)
                {
                    yield break;
                }
                result = value1 % value2;
                break;
            case MathOperation.Power:
                result = (int)Math.Pow(value1, value2);
                break;
            case MathOperation.Maximum:
                result = Math.Max(value1, value2);
                break;
            case MathOperation.Minimum:
                result = Math.Min(value1, value2);
                break;
            case MathOperation.AbsoluteValue:
                result = Math.Abs(value1);
                break;
            case MathOperation.Sign:
                result = Math.Sign(value1);
                break;
            default:
                yield break;
        }
        Action.Registers[(int)StoreResult] = result;
    }
}
