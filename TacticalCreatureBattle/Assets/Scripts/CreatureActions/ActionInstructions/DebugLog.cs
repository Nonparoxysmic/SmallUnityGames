using System.Collections;
using UnityEngine;

public class DebugLog : ActionInstruction
{
    public string Message;

    public override IEnumerator Execute()
    {
        Debug.Log(Message);
        yield break;
    }
}
