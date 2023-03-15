using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Menagerie : MonoBehaviour
{
    public static List<CreatureStats> AllCreatures { get; private set; }
    public static List<CreatureStats> ComputerTeam { get; private set; }
    public static List<CreatureStats> HumanTeam { get; private set; }

    static CreatureStats[] _prebuiltCreatures;
    static Dictionary<string, Species> _species;
    static Dictionary<string, CreatureAction> _actions;

    void Awake()
    {
        // Load and verify all species.
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

        // Load and verify all creature actions.
        CreatureAction[] allActions = Resources.LoadAll<CreatureAction>("");
        _actions = new Dictionary<string, CreatureAction>();
        foreach (CreatureAction action in allActions)
        {
            if (_actions.ContainsKey(action.name))
            {
                this.Error($"Multiple {nameof(CreatureAction)} Resources with the same name: \"{action.name}\"");
                return;
            }
            _actions.Add(action.name, action);
        }

        // Load and verify all creatures.
        _prebuiltCreatures = Resources.LoadAll<CreatureStats>("");
        foreach (CreatureStats creature in _prebuiltCreatures)
        {
            if (!_species.ContainsKey(creature.SpeciesName))
            {
                this.Error($"{nameof(CreatureStats)} Resource with unknown Species: \"{creature.SpeciesName}\"");
                return;
            }
            creature.Species = _species[creature.SpeciesName];
            foreach (string name in creature.MovementActionNames)
            {
                if (!_actions.ContainsKey(name))
                {
                    this.Error($"{nameof(CreatureStats)} Resource with unknown CreatureAction: \"{name}\"");
                    return;
                }
            }
            foreach (string name in creature.BasicActionNames)
            {
                if (!_actions.ContainsKey(name))
                {
                    this.Error($"{nameof(CreatureStats)} Resource with unknown CreatureAction: \"{name}\"");
                    return;
                }
            }
            foreach (string name in creature.SpecialActionNames)
            {
                if (!_actions.ContainsKey(name))
                {
                    this.Error($"{nameof(CreatureStats)} Resource with unknown CreatureAction: \"{name}\"");
                    return;
                }
            }
        }

        // TODO: Load any saved creatures from Application.persistentDataPath folder.

        // Populate the list of all creatures.
        AllCreatures = new List<CreatureStats>();
        for (int i = 0; i < _prebuiltCreatures.Length; i++)
        {
            CreatureStats c = Instantiate(_prebuiltCreatures[i]);
            c.Species = _prebuiltCreatures[i].Species;
            AllCreatures.Add(c);
        }
        // Make sure there are at least 4 creatures.
        while (AllCreatures.Count < 4)
        {
            AllCreatures.Add(CreatureStats.Random());
        }

        // Initialize the team lists.
        HumanTeam = new List<CreatureStats> { AllCreatures[0], AllCreatures[2] };
        ComputerTeam = new List<CreatureStats> { AllCreatures[1], AllCreatures[3] };
    }

    public static Species RandomSpecies()
    {
        return _species.ElementAt(Random.Range(0, _species.Count)).Value;
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

    public static bool TryGetAction(string actionName, out CreatureAction action)
    {
        if (_actions.ContainsKey(actionName))
        {
            action = _actions[actionName];
            return true;
        }
        action = null;
        return false;
    }

    public static string GetActionDisplayName(string actionName)
    {
        if (_actions.ContainsKey(actionName))
        {
            return _actions[actionName].DisplayName;
        }
        return "";
    }

    public static string[] GetActionDisplayNames(string[] actionNames)
    {
        string[] output = new string[actionNames.Length];
        for (int i = 0; i < actionNames.Length; i++)
        {
            output[i] = GetActionDisplayName(actionNames[i]);
        }
        return output;
    }
}
