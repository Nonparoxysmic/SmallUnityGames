using System.Collections;
using UnityEngine;

public class Jump : ActionInstruction
{
    public int JumpValue;
    public bool ValueIsAbsolute;
    public bool ConditionalJump;
    [SerializeField] ValueSource Value1Source;
    public int Value1;
    [SerializeField] ComparisonOperation Comparison;
    [SerializeField] ValueSource Value2Source;
    public int Value2;

    public override IEnumerator Execute()
    {
        if (ConditionalJump)
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
            switch (Comparison)
            {
                case ComparisonOperation.Equals:
                    if (value1 == value2)
                    {
                        DoJump();
                    }
                    break;
                case ComparisonOperation.NotEqual:
                    if (value1 != value2)
                    {
                        DoJump();
                    }
                    break;
                case ComparisonOperation.GreaterThanOrEqual:
                    if (value1 >= value2)
                    {
                        DoJump();
                    }
                    break;
                case ComparisonOperation.GreaterThan:
                    if (value1 > value2)
                    {
                        DoJump();
                    }
                    break;
                case ComparisonOperation.LessThan:
                    if (value1 < value2)
                    {
                        DoJump();
                    }
                    break;
                case ComparisonOperation.LessThanOrEqual:
                    if (value1 <= value2)
                    {
                        DoJump();
                    }
                    break;
                default:
                    yield break;
            }
        }
        else
        {
            DoJump();
        }
    }

    void DoJump()
    {
        if (ValueIsAbsolute)
        {
            if (Action.CurrentInstruction == JumpValue)
            {
                // Do not jump to the current instruction.
                return;
            }
            Action.CurrentInstruction = JumpValue - 1;
            return;
        }
        if (JumpValue == 0)
        {
            // Do not jump to the current instruction.
            return;
        }
        Action.CurrentInstruction += JumpValue - 1;
    }
}
