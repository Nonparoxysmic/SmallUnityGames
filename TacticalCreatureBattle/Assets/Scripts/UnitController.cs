using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour, IComparable<UnitController>
{
    CreatureStats CreatureStats { get; set; }
    Battle Battle { get; set; }

    public Team Team { get; private set; }
    public int UnitID { get; private set; }
    public Size UnitSize { get => CreatureStats.CreatureSize; }
    public Vector3 ViewCenter { get => transform.position + _spriteOffset; }
    public bool InBattle { get; set; }
    public int CurrentHP { get; private set; }
    public int CurrentInitiative { get; private set; }

    SpriteRenderer _spriteRenderer;
    Vector3 _spriteOffset;

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
        _spriteOffset = Vector3.zero;
        switch (UnitSize)
        {
            case Size.Small:
                _spriteOffset = new Vector3(0.5f, 0.25f);
                break;
            case Size.Medium:
                _spriteOffset = new Vector3(0.5f, 0.5f);
                break;
            case Size.Large:
                _spriteOffset = new Vector3(1, 1);
                break;
        }
        spriteGameObject.transform.position += _spriteOffset;
        _spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer.sprite = AssetLibrary.GetSprite(UnitSize, CreatureStats.PrimarySpriteIndex);
        _spriteRenderer.color = CreatureStats.PrimarySpriteColor;
    }

    public void SetVisible(bool isVisible)
    {
        _spriteRenderer.enabled = isVisible;
    }

    /// <summary>
    /// Implements the <seealso cref="IComparable"/> interface.
    /// </summary>
    /// <remarks>
    /// This method sorts elements of the type <seealso cref="UnitController"/> by initiative in 
    /// descending order so that <seealso cref="List{T}.Sort"/> sorts the units into turn order.
    /// </remarks>
    public int CompareTo(UnitController other)
    {
        if (other == null)
        {
            return 1;
        }
        if (CurrentInitiative > other.CurrentInitiative)
        {
            return -1;
        }
        else if (CurrentInitiative == other.CurrentInitiative)
        {
            // TODO: Sort by initiative stat before UnitID.
            if (UnitID < other.UnitID)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        return 1;
    }

    public void IncrementInitiative()
    {
        CurrentInitiative += TurnOrder.INITIATIVE_INCREMENT;
    }

    public void ConsumeInitiative()
    {
        CurrentInitiative -= TurnOrder.INITIATIVE_THRESHOLD;
    }
}
