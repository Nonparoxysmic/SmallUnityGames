using System;

public static class Utilities
{
    static readonly int[,] directions = new int[,] { { 1, 2, 3 }, { 0, -99, 4 }, { 7, 6, 5 } };

    public static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }

    public static bool DirectionsAreAdjacent(int a, int b)
    {
        return Math.Abs(a - b) == 1 || (a == 0 && b == 7) || (b == 0 && a == 7);
    }
}
