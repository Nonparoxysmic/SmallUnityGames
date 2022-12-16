using UnityEngine;

public class UnitController : MonoBehaviour
{
    CreatureStats CreatureStats { get; set; }
    Battle Battle { get; set; }

    public Team Team { get; private set; }
    public int UnitID { get; private set; }
    public int CurrentHP { get; private set; }

    public void Initialize(CreatureStats creatureStats, Battle battle, Team team)
    {
        // Save references.
        CreatureStats = creatureStats;
        Battle = battle;
        // Add unit to battle.
        Team = team;
        UnitID = Battle.Units.Count;
        Battle.Units.Add(this);
        if (Team == Team.Computer)
        {
            Battle.ComputerTeam.Add(this);
        }
        else
        {
            Battle.HumanTeam.Add(this);
        }
        // Set battle-specific stats.
        CurrentHP = CreatureStats.MaximumHP;
        // Create sprites.
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = AssetLibrary.GetSprite(CreatureStats.PrimarySpriteIndex);
        sr.color = CreatureStats.PrimarySpriteColor;
    }
}
