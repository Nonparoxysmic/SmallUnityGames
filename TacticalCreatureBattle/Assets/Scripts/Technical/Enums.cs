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

public enum Element //Change and expand as needed
{
    Element1,
    Element2,
    Element3,
    Element4,
    NoElement
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