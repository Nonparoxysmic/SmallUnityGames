using System;
using System.Text;
using UnityEngine;

public static class Serialization
{
    static readonly char[] _serializationDelimiters = new char[] { ' ', ',', '[', ']', '{', '}' };
    static readonly uint[,] _emptyUint2D = new uint[0, 0];

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

    public static string SerializeArrayUint2D(uint[,] array)
    {
        StringBuilder output = new StringBuilder();
        int width = array.GetLength(0);
        int height = array.GetLength(1);
        output.Append('[');
        output.Append(width);
        output.Append(',');
        output.Append(height);
        output.Append(']');
        output.Append('{');
        for (int x = 0; x < width; x++)
        {
            output.Append('{');
            for (int y = 0; y < height; y++)
            {
                output.Append(array[x, y]);
                if (y < height - 1)
                {
                    output.Append(',');
                }
            }
            output.Append('}');
            if (x < width - 1)
            {
                output.Append(',');
            }
        }
        output.Append('}');
        return output.ToString();
    }

    public static uint[,] DeserializeArrayUint2D(string json)
    {
        string[] terms = json.Split(_serializationDelimiters, StringSplitOptions.RemoveEmptyEntries);
        if (terms.Length < 3)
        {
            Debug.LogError($"{nameof(DeserializeArrayUint2D)}: Insufficent data in JSON string.");
            return _emptyUint2D;
        }
        uint[] numbers = new uint[terms.Length];
        for (int i = 0; i < terms.Length; i++)
        {
            if (!uint.TryParse(terms[i], out numbers[i]))
            {
                Debug.LogError($"{nameof(DeserializeArrayUint2D)}: Invalid data in JSON string: \"{terms[i]}\"");
                return _emptyUint2D;
            }
        }
        uint width = numbers[0];
        uint height = numbers[1];
        if (width == 0 || height == 0)
        {
            Debug.LogError($"{nameof(DeserializeArrayUint2D)}: Zero length array dimension in JSON string.");
            return _emptyUint2D;
        }
        if (width * height + 2 != numbers.Length)
        {
            Debug.LogError($"{nameof(DeserializeArrayUint2D)}: Incorrect number of values in JSON string.");
            return _emptyUint2D;
        }
        uint[,] output = new uint[width, height];
        int index = 2;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                output[x, y] = numbers[index++];
            }
        }
        return output;
    }
}
