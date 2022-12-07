public abstract class ISerializable
{
    // These methods are called by the Serialization class and provide an opportunity 
    // for additional work to be done during the serialization/deserialization process.
    // For example, Unity's built-in UnityEngine.JsonUtility cannot serialize arrays. 
    // So an override of these methods could convert arrays 
    // to and from, respectively, a serializable format, i.e. a string.

    // This method is called prior to JSON serialization to prepare the object to be serialized.
    public virtual void Serialize() { }

    // This method is called after JSON deserialization to prepare the object to be used.
    public virtual void Deserialize() { }
}
