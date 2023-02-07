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
}
