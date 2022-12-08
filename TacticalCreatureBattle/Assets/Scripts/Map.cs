using UnityEngine;

public class Map : ISerializable
{
    // An instance of this class contains a full description of a battle map.
    // This class is deserialized from JSON with the Serialization class.
    // Serialized fields are not prefixed with an underscore.

    [SerializeField] string name = "";
    public string Name { get => name; }

    [SerializeField] string mainLayer = "";
    uint[,] _mainLayer;
    public uint[,] MainLayer { get => _mainLayer; }

    public override void Serialize()
    {
        mainLayer = Serialization.SerializeArrayUint2D(_mainLayer);
    }

    public override void Deserialize()
    {
        _mainLayer = Serialization.DeserializeArrayUint2D(mainLayer);
    }
}
