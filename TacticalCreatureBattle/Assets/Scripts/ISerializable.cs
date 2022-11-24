public abstract class ISerializable
{
    // These methods provide an opportunity for additional work to be done 
    // during the serialization/deserialization process.
    // For example, Unity's built-in UnityEngine.JsonUtility cannot serialize arrays. 
    // So an implementation of these methods could convert arrays 
    // to and from, respectively, a serializable format, i.e. a string.

    // This method is called prior to serialization to prepare the object to be serialized.
    public abstract void Serialize();

    // This method is called after deserialization to prepare the object to be used.
    public abstract void Deserialize();
}
