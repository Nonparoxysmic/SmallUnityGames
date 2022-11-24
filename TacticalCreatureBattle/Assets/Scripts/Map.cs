using UnityEngine;

public class Map : ISerializable
{
    // An instance of this class contains a full description of a battle map.
    // This class is deserialized from JSON with the Serialization class.
    // Serialized fields are not preceded by an underscore.

    [SerializeField] string name;
    public string Name { get => name; }

    public override void Serialize()
    {

    }

    public override void Deserialize()
    {

    }
}
