using System.Collections.Generic;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    // This component holds and manages the data for player and enemy creatures.
    // Other scripts that use or modify this creature data should
    // call this component or access the data by reference.

    public Creature[] DefaultCreatures { get; private set; }

    void Awake()
    {
        DefaultCreatures = GetDefaultCreatures();
        if (DefaultCreatures.Length == 0)
        {
            this.Error("Unable to load default creature resources.");
            return;
        }
    }

    // This method gets the built-in creatures from the JSON files in the project resources.
    Creature[] GetDefaultCreatures()
    {
        List<Creature> creatures = new List<Creature>();
        for (int i = 0; i < 1000; i++)
        {
            TextAsset ta = Resources.Load<TextAsset>($"DefaultCreatures/DefaultCreature{i:D3}");
            if (ta == null)
            {
                continue;
            }
            Creature c = Serialization.FromJson<Creature>(ta.text);
            creatures.Add(c);
        }
        return creatures.ToArray();
    }
}
