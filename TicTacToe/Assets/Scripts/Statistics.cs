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

    public void ResetStatistics()
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

    public SaveData CreateSaveData()
    {
        SaveData save = new SaveData();
        foreach (KeyValuePair<GameDifficulty, int[]> kvp in statistics)
        {
            save.statisticsList.Add(new StatsEntry() { gameDifficulty = kvp.Key, stats = kvp.Value });
        }
        foreach (KeyValuePair<GameDifficulty, GameState> kvp in nextStartingGameState)
        {
            save.nextStartingGameStateList.Add(new StartEntry() { gameDifficulty = kvp.Key, gameState = kvp.Value });
        }
        save.gameInProgress = gameInProgress;
        return save;
    }

    public void LoadData(SaveData save)
    {
        statistics.Clear();
        nextStartingGameState.Clear();
        foreach (StatsEntry entry in save.statisticsList)
        {
            statistics.Add(entry.gameDifficulty, entry.stats);
        }
        foreach (StartEntry entry in save.nextStartingGameStateList)
        {
            nextStartingGameState.Add(entry.gameDifficulty, entry.gameState);
        }
        gameInProgress = save.gameInProgress;
    }

    public int GetStat(GameDifficulty difficulty, int index)
    {
        return statistics[difficulty][index];
    }
}
