using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CreatureAction : MonoBehaviour
{
    public string DisplayName;
    public uint ExecutionLimit = 100;
    public ActionInstruction[] Instructions;

    public bool ActionCompleted { get; private set; }

    public int CurrentInstruction { get; set; }

    public readonly int[] Registers = new int[8];
    public readonly List<UnitController>[] TargetUnits = new List<UnitController>[4];
    public readonly List<Vector2Int>[] TargetCells = new List<Vector2Int>[4];

    uint _executionCount;

    public IEnumerator PerformAction()
    {
        Initialize();
        while (true)
        {
            if (CurrentInstruction < 0 || CurrentInstruction >= Instructions.Length
                || _executionCount >= ExecutionLimit)
            {
                break;
            }
            if (Instructions[CurrentInstruction] != null)
            {
                yield return Instructions[CurrentInstruction].Execute();
            }
            CurrentInstruction++;
            _executionCount++;
        }
        ActionCompleted = true;
    }

    void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            TargetUnits[i] = new List<UnitController>();
            TargetCells[i] = new List<Vector2Int>();
        }
    }
}
