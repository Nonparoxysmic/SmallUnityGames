using System.Collections.Generic;
using UnityEngine;

public class Staging : BattleState
{
    List<SpawnPoint> _computerTeamSpawnPoints;
    List<SpawnPoint> _humanTeamSpawnPoints;

    public override void Enter()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPointObjects.Length == 0)
        {
            this.Error("No spawn points found in level.");
            return;
        }
        _computerTeamSpawnPoints = new List<SpawnPoint>();
        _humanTeamSpawnPoints = new List<SpawnPoint>();
        foreach (GameObject go in spawnPointObjects)
        {
            SpawnPoint spawnPoint = go.GetComponent<SpawnPoint>();
            if (spawnPoint == null || spawnPoint.Size == 0)
            {
                continue;
            }
            if (spawnPoint.Team == Team.Computer)
            {
                _computerTeamSpawnPoints.Add(spawnPoint);
            }
            else
            {
                _humanTeamSpawnPoints.Add(spawnPoint);
            }
        }
        if (_computerTeamSpawnPoints.Count == 0)
        {
            this.Error("No spawn points found in level for Computer team.");
            return;
        }
        if (_humanTeamSpawnPoints.Count == 0)
        {
            this.Error("No spawn points found in level for Human team.");
            return;
        }

        PlaceUnits(Team.Computer);

        KeyboardInput.DirectionalInput += OnDirectionalInput;
    }

    void OnDirectionalInput(object sender, DirectionEventArgs e)
    {

    }

    public override void Exit()
    {
        KeyboardInput.DirectionalInput -= OnDirectionalInput;
    }

    void PlaceUnits(Team team)
    {
        List<SpawnPoint> spawnPoints;
        List<UnitController> unitControllers;
        if (team == Team.Computer)
        {
            spawnPoints = _computerTeamSpawnPoints;
            unitControllers = Battle.ComputerTeam;
        }
        else
        {
            spawnPoints = _humanTeamSpawnPoints;
            unitControllers = Battle.HumanTeam;
        }

        // Remove from battle the units that don't fit on the available spawn points.
        // Units are removed from the end of the list first.
        int removeIndex = unitControllers.Count - 1;
        while (!CanSpawnAll(unitControllers, spawnPoints) && removeIndex > 0)
        {
            unitControllers[removeIndex].SetVisible(false);
            unitControllers[removeIndex].InBattle = false;
            removeIndex--;
        }

        // Sort the units into the spawn points.
        List<UnitController>[] sortedUnits = new List<UnitController>[spawnPoints.Count];
        for (int i = 0; i < sortedUnits.Length; i++)
        {
            sortedUnits[i] = new List<UnitController>();
        }
        int[] capacities = new int[spawnPoints.Count];
        List<int> spawnPairIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            capacities[i] = (int)spawnPoints[i].Size;
            for (int j = 0; j < capacities[i] / 2; j++)
            {
                spawnPairIndices.Add(i);
            }
        }
        spawnPairIndices.Shuffle();
        int pos = 0;
        foreach (UnitController unit in unitControllers)
        {
            if (!unit.InBattle || unit.UnitSize != Size.Large)
            {
                continue;
            }
            int spawnPointIndex = spawnPairIndices[pos++];
            sortedUnits[spawnPointIndex].Add(unit);
            capacities[spawnPointIndex] -= 2;
        }
        List<int> spawnSingleIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            for (int j = 0; j < capacities[i]; j++)
            {
                spawnSingleIndices.Add(i);
            }
        }
        spawnSingleIndices.Shuffle();
        pos = 0;
        foreach (UnitController unit in unitControllers)
        {
            if (!unit.InBattle || unit.UnitSize == Size.Large)
            {
                continue;
            }
            int spawnPointIndex = spawnSingleIndices[pos++];
            sortedUnits[spawnPointIndex].Add(unit);
        }

        // Move the units into the spawn positions.
        for (int i = 0; i < sortedUnits.Length; i++)
        {
            PositionUnits(sortedUnits[i], spawnPoints[i]);
        }
    }

    bool CanSpawnAll(List<UnitController> unitControllers, List<SpawnPoint> spawnPoints)
    {
        int sizeOneUnits = 0;
        int sizeTwoUnits = 0;
        foreach (UnitController unit in unitControllers)
        {
            if (!unit.InBattle)
            {
                continue;
            }
            if (unit.UnitSize == Size.Large)
            {
                sizeTwoUnits++;
            }
            else
            {
                sizeOneUnits++;
            }
        }
        int spawnSingles = 0;
        int spawnPairs = 0;
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.Size % 2 == 1)
            {
                spawnSingles++;
            }
            spawnPairs += (int)spawnPoint.Size / 2;
        }
        if (spawnPairs < sizeTwoUnits)
        {
            return false;
        }
        if (spawnSingles + 2 * (spawnPairs - sizeTwoUnits) < sizeOneUnits)
        {
            return false;
        }
        return true;
    }

    void PositionUnits(List<UnitController> unitControllers, SpawnPoint spawnPoint)
    {
        // TODO: Position the collection of units in the spawn point.
    }
}
