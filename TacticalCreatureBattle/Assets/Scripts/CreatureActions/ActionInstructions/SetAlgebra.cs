using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetAlgebra : ActionInstruction
{
    [SerializeField] ListType Type;
    [SerializeField] ListLabel FirstList;
    [SerializeField] SetOp Operation;
    [SerializeField] ListLabel SecondList;
    [SerializeField] ListLabel StoreResult;

    public override IEnumerator Execute()
    {
        switch (Type)
        {
            case ListType.Units:
                List<UnitController> unitA = Action.TargetUnits[(int)FirstList];
                List<UnitController> unitB = Action.TargetUnits[(int)SecondList];
                Action.TargetUnits[(int)StoreResult] = DoOperation(unitA, unitB);
                yield break;
            case ListType.Cells:
                List<Vector2Int> cellA = Action.TargetCells[(int)FirstList];
                List<Vector2Int> cellB = Action.TargetCells[(int)SecondList];
                Action.TargetCells[(int)StoreResult] = DoOperation(cellA, cellB);
                yield break;
        }
    }

    List<T> DoOperation<T>(List<T> A, List<T> B)
    {
        return Operation switch
        {
            SetOp.Union => (List<T>)A.Union(B),
            SetOp.Intersection => (List<T>)A.Intersect(B),
            SetOp.SetDifference => (List<T>)A.Except(B),
            SetOp.SymmetricDifference => (List<T>)A.Concat(B).Except(A.Intersect(B)),
            _ => A,
        };
    }
}
