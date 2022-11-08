using UnityEngine;

public class Map
{
    // An instance of this class contains a full description of a battle map.
    // This class is deserialized from JSON with UnityEngine.JsonUtility.
    // Serialized fields are not preceded by an underscore.

    [SerializeField] string name;
    public string Name { get => name; }
}
