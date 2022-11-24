using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    public Map Current { get; private set; }
    public Map[] Maps { get; private set; }

    void Awake()
    {
        Maps = GetMaps();
        if (Maps.Length == 0)
        {
            this.Error("Unable to load map resources.");
            return;
        }
        Current = Maps[0];
    }

    // This method gets the built-in battle maps from the JSON files in the project resources.
    Map[] GetMaps()
    {
        List<Map> maps = new List<Map>();
        for (int i = 0; i < 100; i++)
        {
            TextAsset ta = Resources.Load<TextAsset>($"BattleMaps/BattleMap{i:D2}");
            if (ta == null)
            {
                continue;
            }
            Map m = Serialization.FromJson<Map>(ta.text);
            maps.Add(m);
        }
        return maps.ToArray();
    }
}
