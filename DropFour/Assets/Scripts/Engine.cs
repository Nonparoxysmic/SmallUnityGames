using System;

public static class Engine
{
    public static int RandomMove(GameBoard board)
    {
        int[] validMoves = board.ValidMoves();
        return validMoves[UnityEngine.Random.Range(0, validMoves.Length)];
    }

    public static int ChooseMove(GameBoard board, float seconds)
    {
        throw new NotImplementedException();
    }
}
