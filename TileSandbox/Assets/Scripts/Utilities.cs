using System;
using UnityEngine;

public static class Utilities
{
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

    public static int Direction(double vertical, double horizontal)
    {
        if (vertical == 0 && horizontal == 0) { return -8; }
        return (int)(4 * (Math.Atan2(vertical, horizontal) / Math.PI + 1) % 8);
    }

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
