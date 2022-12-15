using UnityEngine;

public class UnitController : MonoBehaviour
{
    public CreatureStats CreatureStats { get; private set; }
    public Battle Battle { get; private set; }

    int _currentHP;

    public void Initialize(CreatureStats creatureStats, Battle battle)
    {
        // Save references.
        CreatureStats = creatureStats;
        Battle = battle;
        // Set battle-specific stats.
        _currentHP = CreatureStats.MaximumHP;
        // Create sprites.
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = AssetLibrary.GetSprite(CreatureStats.PrimarySpriteIndex);
        sr.color = CreatureStats.PrimarySpriteColor;
    }
}
