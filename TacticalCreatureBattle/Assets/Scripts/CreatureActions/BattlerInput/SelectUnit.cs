using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : BattlerInput
{
    [SerializeField] TeamAlignment TeamToSelect;
    public bool SelectFromList;
    [SerializeField] ListLabel SelectionList;
    public bool IncludeActiveUnit;
    public bool ExcludeUnitList;
    [SerializeField] ListLabel ListToExclude;
    [SerializeField] ListLabel StoreResult;

    int _selectionIndex;
    List<UnitController> _units;

    protected override void Initialize()
    {
        List<UnitController> unitPool = Battle.Units;
        switch (TeamToSelect)
        {
            case TeamAlignment.SameTeam:
                unitPool = Battle.ActiveUnit.Team == Team.Human ? Battle.HumanTeam : Battle.ComputerTeam;
                break;
            case TeamAlignment.OpposingTeam:
                unitPool = Battle.ActiveUnit.Team == Team.Human ? Battle.ComputerTeam : Battle.HumanTeam;
                break;
        }
        if (SelectFromList)
        {
            unitPool = Action.TargetUnits[(int)SelectionList];
        }
        _units = new List<UnitController>();
        foreach (UnitController unit in unitPool)
        {
            if (!IncludeActiveUnit && unit.UnitID == Battle.ActiveUnit.UnitID)
            {
                continue;
            }
            if (ExcludeUnitList && Action.TargetUnits[(int)ListToExclude].Contains(unit))
            {
                continue;
            }
            _units.Add(unit);
        }
        if (_units.Count == 0)
        {
            // No eligible units to select.
            Action.TargetUnits[(int)StoreResult] = new List<UnitController>();
            _invalidInput = true;
            return;
        }
        _selectionIndex = 0;
        _cursor.position = new Vector3
            (
                _units[_selectionIndex].ViewCenter.x - 0.5f,
                _units[_selectionIndex].ViewCenter.y - 0.5f,
                _cursor.position.z
            );
        CameraController.LookAtUnit(_units[_selectionIndex]);
    }

    protected override void OnDirectionalInput(object sender, DirectionEventArgs e)
    {
        if (Battle.UI.IsPaused)
        {
            return;
        }
        if (e.Direction.x < 0 || e.Direction.y < 0)
        {
            _selectionIndex--;
            if (_selectionIndex < 0)
            {
                _selectionIndex = _units.Count - 1;
            }
        }
        else if (e.Direction.x > 0 || e.Direction.y > 0)
        {
            _selectionIndex++;
            _selectionIndex %= _units.Count;
        }
        _cursor.position = new Vector3
            (
                _units[_selectionIndex].ViewCenter.x - 0.5f,
                _units[_selectionIndex].ViewCenter.y - 0.5f,
                _cursor.position.z
            );
        CameraController.LookAtUnit(_units[_selectionIndex]);
    }

    protected override void Resolve()
    {
        Action.TargetUnits[(int)StoreResult] = new List<UnitController>() { _units[_selectionIndex] };
    }
}
