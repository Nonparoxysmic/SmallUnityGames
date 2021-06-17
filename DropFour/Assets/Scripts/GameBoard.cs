using System;

public class GameBoard
{
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
        ulong bit = 1UL << --nextPositions[moveList[--movesMade]];
        bitboards[movesMade & 1] ^= bit;
    }
}
