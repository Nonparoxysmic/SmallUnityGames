using System;
using UnityEngine;

public static class Utilities
{
    static readonly string _logFilePath = Application.persistentDataPath + "/log.txt";

    public static string LogFilePath { get => _logFilePath; }

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
        return (int)(Math.Round(4 * (Math.Atan2(vertical, horizontal) / Math.PI + 1)) % 8);
    }

    public static Vector3 DirectionVector(int direction)
    {
        if (!direction.InRange(0, 8))
        {
            return Vector3.zero;
        }
        return directionVectors[direction];
    }

    public static bool DirectionsAreAdjacent(int a, int b)
    {
        return Math.Abs(a - b) == 1 || (a == 0 && b == 7) || (b == 0 && a == 7);
    }

    public static Sprite BlankSquareSprite(int size, Color color)
    {
        size = Math.Max(1, Math.Abs(size));

        Texture2D texture = new Texture2D(size, size);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();

        Sprite sprite = Sprite.Create
            (
                texture,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f),
                size
            );
        return sprite;
    }
}
