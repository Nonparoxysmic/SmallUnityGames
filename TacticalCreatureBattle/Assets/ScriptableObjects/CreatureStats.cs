using UnityEngine;

[CreateAssetMenu(fileName ="New CreatureStats", menuName = "TacticalCreatureBattle/CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string CreatureName;
    public int MaximumHP;
    public uint PrimarySpriteIndex;
    public Color PrimarySpriteColor;

    public static CreatureStats Random()
    {
        CreatureStats creatureStats = CreateInstance<CreatureStats>();
        creatureStats.CreatureName = RandomCreatureName();
        creatureStats.MaximumHP = UnityEngine.Random.Range(1, 10);
        creatureStats.PrimarySpriteIndex = (uint)UnityEngine.Random.Range(0, 12);
        creatureStats.PrimarySpriteColor = UnityEngine.Random.ColorHSV();
        creatureStats.PrimarySpriteColor.a = 1;
        return creatureStats;
    }

    static string RandomCreatureName()
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
