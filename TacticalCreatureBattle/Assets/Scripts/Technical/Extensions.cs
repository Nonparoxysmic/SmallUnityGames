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
    /// Assigns the given value of type T to each element of the specified two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="array">The array to be filled.</param>
    /// <param name="value">The value to assign to each array element.</param>
    public static void Fill<T>(this T[,] array, T value)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = value;
            }
        }
    }

    /// <summary>
    /// Finds a descendant GameObject by name and gets a Component from it.
    /// </summary>
    /// <remarks>
    /// This method performs a recursive depth-first search through the children 
    /// of the Component on which it is called, until it finds a GameObject with 
    /// the specified name. 
    /// It then returns <seealso cref="Component.GetComponent{T}"/> from that GameObject's Transform.
    /// </remarks>
    /// <returns>
    /// The Component of type T from the named GameObject, 
    /// or null if either the named GameObject or the Component are not found.
    /// </returns>
    /// <typeparam name="T">The type of Component to be found.</typeparam>
    /// <param name="component">The Component whose children to search.</param>
    /// <param name="name">The name of the GameObject to be found.</param>
    public static T FindComponentInChildren<T>(this Component component, string name) where T : Component
    {
        Transform nameMatchChild = component.transform.Find(name);
        if (nameMatchChild != null)
        {
            return nameMatchChild.GetComponent<T>();
        }
        for (int i = 0; i < component.transform.childCount; i++)
        {
            Transform childTransform = component.transform.GetChild(i);
            T recursiveResult = childTransform.FindComponentInChildren<T>(name);
            if (recursiveResult != null)
            {
                return recursiveResult;
            }
        }
        return null;
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
    /// Randomly sorts the elements of an <seealso cref="IList{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method shuffles into a random order the elements of a list, array, 
    /// or other collection that implements <seealso cref="IList{T}"/>.
    /// </para>
    /// <para>
    /// The elements are shuffled with a Fisher-Yates algorithm, using 
    /// <seealso cref="UnityEngine"/>.<seealso cref="Random.Range(int, int)"/> 
    /// to generate the randomization.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="iList">The collection to be shuffled.</param>
    public static void Shuffle<T>(this IList<T> iList)
    {
        for (int i = 0; i < iList.Count - 1; i++)
        {
            int j = Random.Range(i, iList.Count);
            T temp = iList[i];
            iList[i] = iList[j];
            iList[j] = temp;
        }
    }
}
