using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool EqualsOneOf(this int N, int a, int b, int c, int d)
    {
        return N == a || N == b || N == c || N == d;
    }

    public static void Error(this Component component, string message)
    {
        Debug.LogError(component.name + ": " + component.GetType() + ": " + message);
        if (component is Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
    }

    public static bool Remove<T>(this Queue<T> queue, T itemToRemove)
    {
        bool itemFound = false;
        int count = queue.Count;
        for (int i = 0; i < count; i++)
        {
            T item = queue.Dequeue();
            if (!itemFound && EqualityComparer<T>.Default.Equals(item, itemToRemove))
            {
                itemFound = true;
            }
            else
            {
                queue.Enqueue(item);
            }
        }
        return itemFound;
    }

    public static int Floor(this int input, int significance)
    {
        return ((float)input).Floor(significance);
    }

    public static int Floor(this float input, int significance)
    {
        if (significance == 0) { return 0; }
        return (int)Math.Floor(input / significance) * significance;
    }
}
