using System.Collections;
using System.Linq;
using UnityEngine;

public class DebugLogRegisters : ActionInstruction
{
    [SerializeField] RegisterType Type;
    [SerializeField] ListLabel ListID;

    public override IEnumerator Execute()
    {
        switch (Type)
        {
            case RegisterType.IntegerRegister:
                PrintRegisters();
                break;
            case RegisterType.UnitsList:
                PrintUnits(ListID);
                break;
            case RegisterType.CellsList:
                PrintCells(ListID);
                break;
        }
        yield break;
    }

    void PrintRegisters()
    {
        Debug.Log($"Register Values: {string.Join(", ", Action.Registers)}");
    }

    void PrintUnits(ListLabel listID)
    {
        string unitIDs = string.Join(", ", Action.TargetUnits[(int)listID].Select(u => u.UnitID));
        Debug.Log($"Unit IDs in Target Units List {listID}: {unitIDs}");
    }

    void PrintCells(ListLabel listID)
    {
        string cells = string.Join(", ", Action.TargetCells[(int)listID]);
        Debug.Log($"Cells in Target Cells List {listID}: {cells}");
    }
}
