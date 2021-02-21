using System;
using System.Collections.Generic;

public class SaveData
{
    public List<StatsEntry> statisticsList;
    public List<StartEntry> nextStartingGameStateList;
    public bool gameInProgress;

    public SaveData()
    {
        statisticsList = new List<StatsEntry>();
        nextStartingGameStateList = new List<StartEntry>();
    }
}

public class StatsEntry
{
    public GameDifficulty gameDifficulty;
    public int[] stats;
}

public class StartEntry
{
    public GameDifficulty gameDifficulty;
    public GameState gameState;
}
