using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New CreatureStats", menuName = "TacticalCreatureBattle/CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string IndividualName;
    public string SpeciesName;

    public Species Species { get; set; }

    [SerializeField]
    private int _MaximumHP;

    [SerializeField]
    private int _Strength;

    [SerializeField]
    private int _Magic;

    [SerializeField]
    private int _Defense;

    [SerializeField]
    private int _Speed;

    public string[] MovementActionNames; // Should we limit the number of actions of each type?
    public string[] BasicActionNames; 
    public string[] SpecialActionNames;

    //Getters for each stat automatically load in the species stat as well. Expanding the getters could also allow for reading stats from passive effects, buffs, or debuffs.
    public int MaximumHP
    {
        get => Species.MaximumHP + _MaximumHP;
        set
        {
            _MaximumHP = value;
        }
    }
    public int Strength
    {
        get => Species.Strength + _Strength;
        set
        {
            _Strength = value;
        }
    }
    public int Magic
    {
        get => Species.Magic + _Magic;
        set
        {
            _Magic = value;
        }
    }
    public int Defense
    {
        get => Species.Defense + _Defense;
        set
        {
            _Defense = value;
        }
    }
    public int Speed
    {
        get => Species.Speed + _Speed;
        set
        {
            _Speed = value;
        }
    }

    public int GetStat(Stat StatIndex)
    {
        switch (StatIndex)
        {
            case Stat.Health:
                return MaximumHP;
            case Stat.Strength:
                return Strength;
            case Stat.Magic:
                return Magic;
            case Stat.Defense:
                return Defense;
            case Stat.Speed:
                return Speed;
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
        creatureStats.MaximumHP = UnityEngine.Random.Range(0, 5);
        creatureStats.Strength = UnityEngine.Random.Range(0, 5);
        creatureStats.Magic = UnityEngine.Random.Range(0, 5);
        creatureStats.Defense = UnityEngine.Random.Range(0, 5);
        creatureStats.Speed = UnityEngine.Random.Range(0, 5);
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
