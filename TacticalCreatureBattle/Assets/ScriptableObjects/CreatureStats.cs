using UnityEngine;

[CreateAssetMenu(fileName ="New CreatureStats", menuName = "TacticalCreatureBattle/CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string IndividualName;
    public string SpeciesName;
    public int MaximumHP;

    public Species Species { get; set; }

    public static CreatureStats Random()
    {
        CreatureStats creatureStats = CreateInstance<CreatureStats>();
        creatureStats.IndividualName = RandomName();
        creatureStats.Species = Menagerie.RandomSpecies();
        creatureStats.SpeciesName = creatureStats.Species.name;
        creatureStats.MaximumHP = UnityEngine.Random.Range(1, 10);
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
