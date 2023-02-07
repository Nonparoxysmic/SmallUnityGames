using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatTotalConditional
{
    public Stat StatToCompare;
    public Comparison ComparisonToUse;
    public int CompareTo;

    public bool IsMet(CreatureStats creature)
    {
        int creatureStat = creature.GetStatTotal(StatToCompare);
        if (creatureStat == -1)
        {
            return false;
        }

        switch (ComparisonToUse)
        {
            case Comparison.GreaterThan:
                if (creatureStat > CompareTo)
                {
                    return true;
                }
                break;
            case Comparison.LessThan:
                if (creatureStat < CompareTo)
                {
                    return true;
                }
                break;
            case Comparison.EqualTo:
                if (creatureStat == CompareTo)
                {
                    return true;
                }
                break;
            case Comparison.DivisibleBy:
                if (creatureStat % CompareTo == 0)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
}