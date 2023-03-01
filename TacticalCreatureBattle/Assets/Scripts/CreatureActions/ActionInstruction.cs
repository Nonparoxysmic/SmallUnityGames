using System.Collections;
using UnityEngine;

public abstract class ActionInstruction : MonoBehaviour
{
    public static Battle Battle { get; set; }

    public static CreatureAction Action { get => Battle.CurrentAction; }

    public abstract IEnumerator Execute();

    protected enum RegisterType
    {
        IntegerRegister,
        UnitsList,
        CellsList
    }

    protected enum RegisterLabel
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H
    }

    protected enum ListLabel
    {
        A,
        B,
        C,
        D
    }

    protected enum ValueSource
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        Value
    }

    protected enum MathOperation
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Power,
        Maximum,
        Minimum,
        AbsoluteValue,
        Sign
    }

    protected enum ComparisonOperation
    {
        Equals,
        NotEqual,
        GreaterThanOrEqual,
        GreaterThan,
        LessThan,
        LessThanOrEqual
    }

    protected enum SourceType
    {
        ActiveUnit,
        TargetUnit,
        TargetCell
    }
}
