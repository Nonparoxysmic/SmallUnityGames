using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearnableAction
{
    public CreatureAction ActionToLearn;
    public int RequiredConditionals = -1; //If negative or zero, require all conditionals on the list
    public StatTotalConditional[] StatTotalConditionals;
    
    public bool CanLearnAction(CreatureStats creature)
    {
        if (StatTotalConditionals.Length < 1)
        {
            return true;
        }
        int counted = 0;
        for (int i = 0; i < StatTotalConditionals.Length; i++)
        {
            if (StatTotalConditionals[i].IsMet(creature))
            {
                counted += 1;
            }
        }
        if (RequiredConditionals < 1 && counted >= StatTotalConditionals.Length)
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