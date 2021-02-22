using System;
using UnityEngine;

public class Engine : MonoBehaviour
{
    static GameMasterScript gm;
    static Letter evaluatorLetter;
    static Letter opponentLetter;
    static int evalSign = 1;

    private void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }

    public static int WorstMove(Letter letterToPlay, Letter[] grid)
    {
        evalSign = -1;
        return BestMove(letterToPlay, grid);
    }

    public static int BestMove(Letter letterToPlay, Letter[] grid)
    {
        evaluatorLetter = letterToPlay;
        opponentLetter = (Letter)((int)evaluatorLetter % 2 + 1);

        int bestmove = -1;
        int value = -2;
        for (int i = 0; i < grid.Length; i++)
        {
            if (grid[i] == Letter.Blank)
            {
                Letter[] nextGrid = GetNextGrid(evaluatorLetter, i, grid);
                int moveValue = Minimax(evaluatorLetter, nextGrid);
                if (moveValue > value)
                {
                    value = moveValue;
                    bestmove = i;
                }
            }
        }
        evalSign = 1;
        return bestmove;
    }

    static int Minimax(Letter letterPlayed, Letter[] grid)
    {
        if (CanEvaluateGrid(letterPlayed, grid, out int eval))
        {
            return eval * evalSign;
        }
        if (letterPlayed == opponentLetter)
        {
            int value = -2;
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == Letter.Blank)
                {
                    Letter[] nextGrid = GetNextGrid(evaluatorLetter, i, grid);
                    value = Math.Max(value, Minimax(evaluatorLetter, nextGrid));
                }
            }
            return value;
        }
        else
        {
            int value = 2;
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == Letter.Blank)
                {
                    Letter[] nextGrid = GetNextGrid(opponentLetter, i, grid);
                    value = Math.Min(value, Minimax(opponentLetter, nextGrid));
                }
            }
            return value;
        }
    }

    static Letter[] GetNextGrid(Letter letterToPlay, int move, Letter[] grid)
    {
        Letter[] nextGrid = (Letter[])grid.Clone();
        nextGrid[move] = letterToPlay;
        return nextGrid;
    }

    static bool CanEvaluateGrid(Letter letterPlayed, Letter[] grid, out int eval)
    {
        if (FindGameOver(letterPlayed, grid, out GameResult result))
        {
            if (result == GameResult.Win && letterPlayed == evaluatorLetter)
            {
                eval = 1;
            }
            else if (result == GameResult.Win && letterPlayed == opponentLetter)
            {
                eval = -1;
            }
            else
            {
                eval = 0;
            }
            return true;
        }
        else
        {
            eval = 0;
            return false;
        }
    }

    static bool FindGameOver(Letter letterPlayed, Letter[] grid, out GameResult result)
    {
        for (int line = 0; line < 8; line++)
        {
            gm.CheckLine(letterPlayed, grid, line, out int goodBoxes, out _, out _);
            if (goodBoxes == 3)
            {
                result = GameResult.Win;
                return true;
            }
        }
        bool noBlankSpaces = true;
        foreach (Letter box in grid)
        {
            if (box == Letter.Blank) noBlankSpaces = false;
        }
        if (noBlankSpaces)
        {
            result = GameResult.Draw;
            return true;
        }
        result = GameResult.Draw;
        return false;
    }
}
