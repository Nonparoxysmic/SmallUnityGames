using System.Collections;
using UnityEngine;

public class SetFlags : ActionInstruction
{
    [SerializeField] ActiveUnitFlag Flag;
    public bool SetTo;

    public override IEnumerator Execute()
    {
        switch (Flag)
        {
            case ActiveUnitFlag.HasMoved:
                Battle.ActiveUnit.HasMoved = SetTo;
                break;
            case ActiveUnitFlag.HasBasicAttacked:
                Battle.ActiveUnit.HasBasicAttacked = SetTo;
                break;
        }
        yield break;
    }
}
