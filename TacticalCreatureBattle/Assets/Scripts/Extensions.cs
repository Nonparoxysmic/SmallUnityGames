using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Logs an error message to the Unity console and disables the Component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses <seealso cref="Debug.LogError(object, Object)"/> to 
    /// log the error message to the Unity console along with the name 
    /// of the GameObject to which the Component is attached. If the Component 
    /// is a Behaviour, which it should be, it is disabled.
    /// </para>
    /// <para>
    /// This method should be called in a Unity MonoBehavior and usually 
    /// followed immediately by returning from the current method, as below.
    /// </para>
    /// <code>
    /// this.Error("Error message here.");
    /// return;
    /// </code>
    /// </remarks>
    /// <param name="component">The Component that encountered an error.</param>
    /// <param name="message">The error message to log to the Unity console.</param>
    public static void Error(this Component component, string message)
    {
        Debug.LogError($"{component.gameObject.name}: {message}", component.gameObject);
        if (component is Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
    }
}
