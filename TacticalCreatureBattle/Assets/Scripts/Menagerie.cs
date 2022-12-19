using System;
using System.Collections.Generic;
using UnityEngine;

public class Menagerie : MonoBehaviour
{
    [SerializeField] CreatureStats[] _prebuiltCreatures;

    public static CreatureStats[] ComputerTeam { get; private set; }
    public static CreatureStats[] HumanTeam { get; private set; }

    static Menagerie _instance;

    void Awake()
    {
        _instance = this;
        if (_prebuiltCreatures == null)
        {
            this.Error("Serialized field is null.");
            return;
        }
        if (_prebuiltCreatures.Length == 0)
        {
            this.Error($"No prebuilt creatures in {typeof(Menagerie)} array.");
            return;
        }
        for (int i = 0; i < _prebuiltCreatures.Length; i++)
        {
            if (_prebuiltCreatures[i] == null)
            {
                this.Error($"Missing reference in inspector: \"{nameof(_prebuiltCreatures)}\" array.");
                return;
            }
        }

        // TODO: Load any saved creatures from Application.persistentDataPath folder.

        // Initialize the team arrays.
        InitializeTeamArrays();
    }

    void InitializeTeamArrays()
    {
        CreatureStats[] creatures = new CreatureStats[4];
        for (int i = 0; i < Math.Min(_prebuiltCreatures.Length, 4); i++)
        {
            creatures[i] = Instantiate(_prebuiltCreatures[i]);
        }
        for (int i = _prebuiltCreatures.Length; i < 4; i++)
        {
            creatures[i] = CreatureStats.Random();
        }
        HumanTeam = new CreatureStats[] { creatures[0], creatures[2] };
        ComputerTeam = new CreatureStats[] { creatures[1], creatures[3] };
    }

    public static CreatureStats[] GetPrebuiltCreatures()
    {
        List<CreatureStats> creatureStats = new List<CreatureStats>();
        foreach (CreatureStats cs in _instance._prebuiltCreatures)
        {
            creatureStats.Add(Instantiate(cs));
        }
        return creatureStats.ToArray();
    }

    public static GameObject CreateUnit(CreatureStats creatureStats, Battle battle, Team team, Transform parent = null)
    {
        GameObject unit = new GameObject();
        if (parent != null)
        {
            unit.transform.parent = parent;
        }
        UnitController uc = unit.AddComponent<UnitController>();
        uc.Initialize(creatureStats, battle, team);
        unit.name = $"{team} Team -- Unit {uc.UnitID} -- {creatureStats.CreatureName}";
        return unit;
    }
}
