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
        return StatIndex switch
        {
            Stat.Health   => _Health,
            Stat.Strength => _Strength,
            Stat.Magic    => _Magic,
            Stat.Defense  => _Defense,
            Stat.Speed    => _Speed,
            _ => -1,
        };
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
        return StatIndex switch
        {
            Stat.Health   => _Health + Species.Health,
            Stat.Strength => _Strength + Species.Strength,
            Stat.Magic    => _Magic + Species.Magic,
            Stat.Defense  => _Defense + Species.Defense,
            Stat.Speed    => _Speed + Species.Speed,
            _ => -1,
        };
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
        creatureStats.MovementActionNames = new string[] { "Move_Step" };
        creatureStats.BasicActionNames = new string[] { "Basic_Slap" };
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
