using UnityEngine;

public class UnitController : MonoBehaviour
{
    CreatureStats CreatureStats { get; set; }
    Battle Battle { get; set; }

    public Team Team { get; private set; }
    public int UnitID { get; private set; }
    public Size UnitSize { get => CreatureStats.CreatureSize; }
    public bool InBattle { get; set; }
    public int CurrentHP { get; private set; }

    SpriteRenderer _spriteRenderer;

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
        InBattle = true;
        CurrentHP = CreatureStats.MaximumHP;
        // Create sprites.
        GameObject spriteGameObject = new GameObject
        {
            name = "Sprite"
        };
        spriteGameObject.transform.parent = transform;
        switch (UnitSize)
        {
            case Size.Small:
                spriteGameObject.transform.position += new Vector3(0.5f, 0.25f);
                break;
            case Size.Medium:
                spriteGameObject.transform.position += new Vector3(0.5f, 0.5f);
                break;
            case Size.Large:
                spriteGameObject.transform.position += new Vector3(1, 1);
                break;
        }
        _spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetLibrary.GetSprite(UnitSize, CreatureStats.PrimarySpriteIndex);
        _spriteRenderer.color = CreatureStats.PrimarySpriteColor;
    }

    public void SetVisible(bool isVisible)
    {
        _spriteRenderer.enabled = isVisible;
    }
}
