using UnityEngine;

public static class Serialization
{
    // Serialize an ISerializable object to a JSON string.
    public static string ToJson<T>(T serializable) where T : ISerializable
    {
        serializable.Serialize();
        return JsonUtility.ToJson(serializable);
    }

    // Deserialize a JSON string to an ISerializable object.
    public static T FromJson<T>(string json) where T : ISerializable
    {
        T result = JsonUtility.FromJson<T>(json);
        result.Deserialize();
        return result;
    }
}
