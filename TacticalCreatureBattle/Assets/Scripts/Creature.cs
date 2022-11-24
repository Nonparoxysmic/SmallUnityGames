using UnityEngine;

public class Creature : ISerializable
{
    // An instance of this class contains the characteristics and stats of a single individual creature.
    // This class is serialized to and deserialized from JSON with the Serialization class.
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
