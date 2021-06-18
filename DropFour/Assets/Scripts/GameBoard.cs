using System;

public class GameBoard
{
    readonly int[] bitShiftDirections = new int[] { 1, 7, 8, 9 };
    readonly int[] aboveTopRow = new int[] { 6, 14, 22, 30, 38, 46, 54 };
    readonly int[] printOrderedPositions = new int[] {5, 13, 21, 29, 37, 45, 53,
        4, 12, 20, 28, 36, 44, 52, 3, 11, 19, 27, 35, 43, 51, 2, 10, 18, 26, 34, 42, 50,
        1, 9, 17, 25, 33, 41, 49, 0, 8, 16, 24, 32, 40, 48 };

    //  5  13  21  29  37  45  53
    //  4  12  20  28  36  44  52
    //  3  11  19  27  35  43  51
    //  2  10  18  26  34  42  50
    //  1   9  17  25  33  41  49
    //  0   8  16  24  32  40  48
    readonly ulong[] bitboards;

    readonly int[] nextPositions;
    readonly int[] moveList;
    int movesMade;

    public GameBoard()
    {
        bitboards = new ulong[2];
        nextPositions = new int[] { 0, 8, 16, 24, 32, 40, 48 };
        moveList = new int[42];
    }

    public void MakeMove(int column)
    {
        bitboards[movesMade & 1] |= 1UL << nextPositions[column]++;
        moveList[movesMade++] = column;
    }

    public void UnmakeLastMove()
    {
        --movesMade;
        bitboards[movesMade & 1] ^= 1UL << --nextPositions[moveList[movesMade]];
    }

    public bool IsValidMove(int column)
    {
        return nextPositions[column] < aboveTopRow[column];
    }

    public int[] ValidMoves()
    {
        int count = 0;
        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(col)) count++;
        }
        int[] output = new int[count];
        int index = 0;
        for (int col = 0; col < 7; col++)
        {
            if (IsValidMove(col))
            {
                output[index] = col;
                index++;
            }
        }
        return output;
    }

    public bool HasConnectedFour(int player)
    {
        foreach (int direction in bitShiftDirections)
        {
            ulong shift = bitboards[player] & (bitboards[player] >> direction);
            if ((shift & (shift >> (2 * direction))) != 0) return true;
        }
        return false;
    }

    public int[] PositionData()
    {
        int[] output = new int[85];
        for (int i = 0; i < 42; i++)
        {
            output[i] = (int)(bitboards[0] >> printOrderedPositions[i]) & 1;
            output[i + 42] = (int)(bitboards[1] >> printOrderedPositions[i]) & 1;
        }
        output[84] = movesMade & 1;
        return output;
    }
}
