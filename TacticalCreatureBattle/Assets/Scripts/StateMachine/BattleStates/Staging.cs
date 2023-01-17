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
            if (spawnPoint == null)
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

        PlaceComputerTeamUnits();

        KeyboardInput.DirectionalInput += OnDirectionalInput;
    }

    void OnDirectionalInput(object sender, DirectionEventArgs e)
    {

    }

    public override void Exit()
    {
        KeyboardInput.DirectionalInput -= OnDirectionalInput;
    }

    void PlaceComputerTeamUnits()
    {
        // TODO: Place the units of the computer team.
    }
}
