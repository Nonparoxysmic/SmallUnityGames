public enum Comparison
{
    GreaterThan,
    LessThan,
    EqualTo,
    DivisibleBy
}

public enum Direction
{
    Left,
    Down,
    Right,
    Up
}

public enum Element
{
    NoElement,
    Aluminum,
    Cardboard,
    Glass,
    Paper,
    Plastic,
    Steel
}

public enum GridWallsTool
{
    None,
    Toggle,
    Erase,
    Wall,
    FallStop
}

public enum Size
{
    Small,
    Medium,
    Large
}

public enum Stat //For use in situations where an effect might test multiple stats.
{
    Health,
    Strength,
    Magic,
    Defense,
    Speed
}

public enum Team
{
    Computer,
    Human
}

public enum TeamAlignment
{
    OpposingTeam,
    SameTeam,
    Both
}

public enum BattleWinner
{
    TBD,
    Computer,
    Human,
    Tie
}
