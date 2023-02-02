using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Menagerie : MonoBehaviour
{
    public static CreatureStats[] ComputerTeam { get; private set; }
    public static CreatureStats[] HumanTeam { get; private set; }

    static CreatureStats[] _prebuiltCreatures;
    static Dictionary<string, Species> _species;

    void Awake()
    {
        Species[] allSpecies = Resources.LoadAll<Species>("");
        _species = new Dictionary<string, Species>();
        foreach (Species species in allSpecies)
        {
            if (_species.ContainsKey(species.name))
            {
                this.Error($"Multiple {nameof(Species)} Resources with the same name: \"{species.name}\"");
                return;
            }
            _species.Add(species.name, species);
        }
        if (_species.Count == 0)
        {
            this.Error($"No {nameof(Species)} found.");
            return;
        }

        _prebuiltCreatures = Resources.LoadAll<CreatureStats>("");
        foreach (CreatureStats creature in _prebuiltCreatures)
        {
            if (!_species.ContainsKey(creature.SpeciesName))
            {
                this.Error($"{nameof(CreatureStats)} Resource with unknown Species: \"{creature.SpeciesName}\"");
                return;
            }
            creature.Species = _species[creature.SpeciesName];
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
            creatures[i].Species = _prebuiltCreatures[i].Species;
        }
        for (int i = _prebuiltCreatures.Length; i < 4; i++)
        {
            creatures[i] = CreatureStats.Random();
        }
        HumanTeam = new CreatureStats[] { creatures[0], creatures[2] };
        ComputerTeam = new CreatureStats[] { creatures[1], creatures[3] };
    }

    public static Species RandomSpecies()
    {
        return _species.ElementAt(UnityEngine.Random.Range(0, _species.Count)).Value;
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
        unit.name = $"{team} Team -- Unit {uc.UnitID} -- {creatureStats.IndividualName}";
        return unit;
    }
}
