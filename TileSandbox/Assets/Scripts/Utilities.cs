using System;
using UnityEngine;

public static class Utilities
{
    public static void ComponentError(Component component, string message)
    {
        Debug.LogError(component.name + ": " + component.GetType() + ": " + message);
        if (component is Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
    }

    static readonly Vector3[] directionVectors = new Vector3[]
    {
        new Vector3(-1, 0, 0),
        new Vector3(-0.7071068f, -0.7071068f, 0),
        new Vector3(0, -1, 0),
        new Vector3(0.7071068f, -0.7071068f, 0),
        new Vector3(1, 0, 0),
        new Vector3(0.7071068f, 0.7071068f, 0),
        new Vector3(0, 1, 0),
        new Vector3(-0.7071068f, 0.7071068f, 0)
    };

    public static Vector3 DirectionVector(int direction)
    {
        if (direction < 0 || direction >= 8)
        {
            return Vector3.zero;
        }
        return directionVectors[direction];
    }

    public static bool DirectionsAreAdjacent(int a, int b)
    {
        return Math.Abs(a - b) == 1 || (a == 0 && b == 7) || (b == 0 && a == 7);
    }
}
