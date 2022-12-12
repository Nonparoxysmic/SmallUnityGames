using UnityEngine;

public static class Serialization
{
    public static string Serialize(CreatureStats creatureStats)
    {
        return JsonUtility.ToJson(creatureStats);
    }

    public static CreatureStats Deserialize<CreatureStats>(string json)
    {
        return JsonUtility.FromJson<CreatureStats>(json);
    }
}
