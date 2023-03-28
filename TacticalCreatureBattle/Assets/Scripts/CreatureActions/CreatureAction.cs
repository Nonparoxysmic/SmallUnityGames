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
    public bool ActionCanceled { get; set; }
    public bool TurnEnded { get; set; }

    public int CurrentInstruction { get; set; }

    bool _instructionSuccess;
    bool _setFalse;
    public bool InstructionSuccess
    {
        get => _instructionSuccess;
        set
        {
            _instructionSuccess = value;
            if (!_instructionSuccess)
            {
                _setFalse = true;
            }
        }
    }

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
                || _executionCount >= ExecutionLimit || ActionCanceled)
            {
                break;
            }
            _setFalse = false;
            if (Instructions[CurrentInstruction] != null)
            {
                yield return Instructions[CurrentInstruction].Execute();
            }
            if (!_setFalse)
            {
                _instructionSuccess = true;
            }
            CurrentInstruction++;
            _executionCount++;
        }
        ActionCompleted = true;
    }

    void Initialize()
    {
        _executionCount = 0;
        CurrentInstruction = 0;
        InstructionSuccess = true;
        for (int i = 0; i < 4; i++)
        {
            TargetUnits[i] = new List<UnitController>();
            TargetCells[i] = new List<Vector2Int>();
        }
    }
}
