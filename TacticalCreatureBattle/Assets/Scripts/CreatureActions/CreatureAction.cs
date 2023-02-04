using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class CreatureAction : MonoBehaviour
{
    public string DisplayName;
    public uint ExecutionLimit = 100;
    public ActionInstruction[] Instructions;

    public bool ActionCompleted { get; private set; }

    public int CurrentInstruction { get; set; }

    uint _executionCount;

    public IEnumerator PerformAction()
    {
        while (true)
        {
            if (CurrentInstruction < 0 || CurrentInstruction >= Instructions.Length
                || _executionCount >= ExecutionLimit)
            {
                break;
            }
            yield return Instructions[CurrentInstruction].Execute();
            CurrentInstruction++;
            _executionCount++;
        }
        ActionCompleted = true;
    }
}
