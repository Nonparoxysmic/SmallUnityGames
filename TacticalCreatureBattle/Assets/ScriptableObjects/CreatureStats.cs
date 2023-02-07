using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New CreatureStats", menuName = "TacticalCreatureBattle/CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string IndividualName;
    public string SpeciesName;

    public Species Species { get; set; }

    [SerializeField]
    private int _Health;

    [SerializeField]
    private int _Strength;

    [SerializeField]
    private int _Magic;

    [SerializeField]
    private int _Defense;

    [SerializeField]
    private int _Speed;

    public int MaximumHP { get => GetStatTotal(Stat.Health);  }

    public string[] MovementActionNames; // Should we limit the number of actions of each type?
    public string[] BasicActionNames; 
    public string[] SpecialActionNames;

    //Getter for personal stats

    public int GetStatIndividual(Stat StatIndex)
    {
        switch (StatIndex)
        {
            case Stat.Health:
                return _Health;
            case Stat.Strength:
                return _Strength;
            case Stat.Magic:
                return _Magic;
            case Stat.Defense:
                return _Defense;
            case Stat.Speed:
                return _Speed;
            default:
                return -1;
        }
    }

    //Setter for personal stats; it enforces a minimum of zero to prevent unexpected behavior until we decide what the stats actually do.

    public void SetStatIndividual(Stat StatIndex, int newValue)
    {
        switch (StatIndex)
        {
            case Stat.Health:
                _Health = (newValue >= 0) ? newValue: 0;
                return;
            case Stat.Strength:
                _Strength = (newValue >= 0) ? newValue : 0;
                return;
            case Stat.Magic:
                _Magic = (newValue >= 0) ? newValue : 0;
                return;
            case Stat.Defense:
                _Defense = (newValue >= 0) ? newValue : 0;
                return;
            case Stat.Speed:
                _Speed = (newValue >= 0) ? newValue : 0;
                return;
            default:
                return;
        }
    }

    public int GetStatTotal(Stat StatIndex)
    {
        switch (StatIndex)
        {
            case Stat.Health:
                return _Health + Species.Health;
            case Stat.Strength:
                return _Strength + Species.Strength;
            case Stat.Magic:
                return _Magic + Species.Magic;
            case Stat.Defense:
                return _Defense + Species.Defense;
            case Stat.Speed:
                return _Speed + Species.Defense;
            default:
                return -1;
        }
    }

    public static CreatureStats Random()
    {
        CreatureStats creatureStats = CreateInstance<CreatureStats>();
        creatureStats.IndividualName = RandomName();
        creatureStats.Species = Menagerie.RandomSpecies();
        creatureStats.SpeciesName = creatureStats.Species.name;
        creatureStats._Health = UnityEngine.Random.Range(0, 5);
        creatureStats._Strength = UnityEngine.Random.Range(0, 5);
        creatureStats._Magic = UnityEngine.Random.Range(0, 5);
        creatureStats._Defense = UnityEngine.Random.Range(0, 5);
        creatureStats._Speed = UnityEngine.Random.Range(0, 5);
        creatureStats.MovementActionNames = Array.Empty<string>();
        creatureStats.BasicActionNames = Array.Empty<string>();
        creatureStats.SpecialActionNames = Array.Empty<string>();
        return creatureStats;
    }

    static string RandomName()
    {
        char[] name = new char[3];
        name[0] = (char)UnityEngine.Random.Range(65, 91);
        for (int i = 1; i < name.Length; i++)
        {
            name[i] = (char)UnityEngine.Random.Range(97, 123);
        }
        return string.Join("", name);
    }
}
