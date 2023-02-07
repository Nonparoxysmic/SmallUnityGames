using System.Collections;
using UnityEngine;

public class DebugLog : ActionInstruction
{
    public bool AppendRegisterValue;
    public string Message;
    [SerializeField] RegisterLabel Register;

    public override IEnumerator Execute()
    {
        if (AppendRegisterValue)
        {
            Debug.Log(Message + $" {Action.Registers[(int)Register]}");
        }
        else
        {
            Debug.Log(Message);
        }
        yield break;
    }
}
