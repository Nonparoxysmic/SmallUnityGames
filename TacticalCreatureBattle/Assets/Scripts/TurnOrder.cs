using System.Collections.Generic;

public class TurnOrder
{
    public static readonly int INITIATIVE_INCREMENT = 25;
    public static readonly int INITIATIVE_THRESHOLD = 100;

    int index = -1;
    List<UnitController> Units { get; }

    public TurnOrder(ref List<UnitController> units)
    {
        Units = units;
    }

    public UnitController NextUnit()
    {
        if (index == -1)
        {
            Units.Sort();
            index = 0;
        }
        if (index >= Units.Count)
        {
            index = 0;
        }
        if (Units[index].CurrentInitiative >= INITIATIVE_THRESHOLD)
        {
            return Units[index++];
        }
        while (Units[0].CurrentInitiative < INITIATIVE_THRESHOLD)
        {
            IncrementInitiatives();
            Units.Sort();
        }
        index = 1;
        return Units[0];
    }

    void IncrementInitiatives()
    {
        foreach (UnitController unit in Units)
        {
            unit.IncrementInitiative();
        }
    }
}
