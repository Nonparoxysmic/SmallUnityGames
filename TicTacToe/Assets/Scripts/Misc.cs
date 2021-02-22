using UnityEngine.Events;

public class BoxClickedEvent : UnityEvent<int> { }
public class BoxUpdatedEvent : UnityEvent<int, Letter> { }

public enum GameDifficulty
{
    Easiest,
    Easy,
    Medium,
    Hard,
    Hardest
}

public enum GameResult
{
    Win,
    Draw,
    Lose
}

public enum GameState
{
    Start,
    PlayerTurn,
    CompTurn,
    End
}

public enum Letter
{
    Blank,
    X,
    O
}
