using System;
using System.Collections.Generic;

public class Statistics
{
    readonly Dictionary<GameDifficulty, int[]> statistics;
    readonly Dictionary<GameDifficulty, GameState> nextStartingGameState;
    public bool gameInProgress;

    public Statistics()
    {
        statistics = new Dictionary<GameDifficulty, int[]>();
        nextStartingGameState = new Dictionary<GameDifficulty, GameState>();
        ResetStatistics();
    }

    void ResetStatistics()
    {
        statistics.Clear();
        nextStartingGameState.Clear();
        foreach (GameDifficulty difficulty in Enum.GetValues(typeof(GameDifficulty)))
        {
            statistics.Add(difficulty, new int[3]);
            nextStartingGameState.Add(difficulty, (GameState)UnityEngine.Random.Range(1, 3));
        }
    }

    public GameState GetNextGameState(GameDifficulty difficulty)
    {
        return nextStartingGameState[difficulty];
    }

    public void AddGame(GameDifficulty difficulty, GameResult result)
    {
        statistics[difficulty][(int)result]++;
        gameInProgress = false;
        nextStartingGameState[difficulty] = (GameState)(((int)nextStartingGameState[difficulty]) % 2) + 1;
    }

    public string DebugGetStatistics()
    {
        string output = "Debug Statistics" + Environment.NewLine;
        foreach (GameDifficulty difficulty in Enum.GetValues(typeof(GameDifficulty)))
        {
            output += difficulty.ToString() + ": " + statistics[difficulty][0] + " wins, " + statistics[difficulty][1] + " draws, " + statistics[difficulty][2] + " losses" + Environment.NewLine;
        }
        return output;
    }
}
