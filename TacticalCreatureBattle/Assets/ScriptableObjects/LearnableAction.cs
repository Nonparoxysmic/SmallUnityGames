using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearnableAction
{
    public CreatureAction ActionToLearn;
    public int RequiredConditionals = -1; //If negative or zero, require all conditionals on the list
    public Conditional[] Conditionals;
    
    public bool CanLearnAction(CreatureStats creature)
    {
        if (Conditionals.Length <= 1)
        {
            return true;
        }
        int counted = 0;
        for (int i = 0; i < Conditionals.Length; i++)
        {
            if (Conditionals[i].IsMet(creature))
            {
                counted += 1;
            }
        }
        if (RequiredConditionals < 1 && counted >= Conditionals.Length)
        {
            return true;
        }
        else if (counted >= RequiredConditionals)
        {
            return true;
        }
        return false;
    }
}
[System.Serializable]
public class Conditional
{
    public Stat StatToCompare;
    public Comparison ComparisonToUse;
    public int CompareTo;

    public bool IsMet(CreatureStats creature)
    {
        int creatureStat = creature.GetStat(StatToCompare);
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