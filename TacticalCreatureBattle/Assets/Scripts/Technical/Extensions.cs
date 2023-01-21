using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Logs an error message to the Unity console and disables the Component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses <seealso cref="Debug.LogError(object, Object)"/> to 
    /// log the error message to the Unity console along with the names of the 
    /// Component and the GameObject to which it is attached. If the Component 
    /// is a Behaviour, which it should be, it is disabled.
    /// </para>
    /// <para>
    /// This method should be called in a Unity MonoBehavior and usually 
    /// followed immediately by returning from the current method.
    /// </para>
    /// </remarks>
    /// <param name="component">The Component that encountered an error.</param>
    /// <param name="message">The error message to log to the Unity console.</param>
    public static void Error(this Component component, string message)
    {
        string gameObjectName = $"GameObject \"{component.gameObject.name}\"";
        string componentName = $"Component \"{component.GetType()}\"";
        Debug.LogError($"{gameObjectName}: {componentName}: {message}", component.gameObject);
        if (component is Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
    }

    /// <summary>
    /// Returns a new string in which all white space characters from the current string are deleted.
    /// </summary>
    /// <remarks>
    /// White space characters are those identified by
    /// the method <seealso cref="char.IsWhiteSpace(char)"/>.
    /// </remarks>
    /// <returns>
    /// A new string that is equivalent to the current instance
    /// except for the removed characters.
    /// </returns>
    /// <param name="current">The string from which to remove white space characters.</param>
    public static string RemoveAllWhiteSpace(this string current)
    {
        char[] charArray = current.ToCharArray();
        int outputLength = 0;
        for (int i = 0; i < charArray.Length; i++)
        {
            if (!char.IsWhiteSpace(charArray[i]))
            {
                charArray[outputLength++] = charArray[i];
            }
        }
        return new string(charArray, 0, outputLength);
    }

    /// <summary>
    /// Randomly sorts the elements of a list.
    /// </summary>
    /// <remarks>
    /// This method implements a Fisher-Yates shuffle, using 
    /// <seealso cref="Random.Range(int, int)"/> to generate the randomization.
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="list">The list to be shuffled.</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int j = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
