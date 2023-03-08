using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour, IComparable<UnitController>
{
    public CreatureStats CreatureStats { get; private set; }
    Battle Battle { get; set; }

    public Team Team { get; private set; }
    public int UnitID { get; private set; }
    public Size UnitSize { get => CreatureStats.Species.Size; }
    public string[] MovementActionNames { get => CreatureStats.MovementActionNames; }
    public string[] BasicActionNames { get => CreatureStats.BasicActionNames; }
    public string[] SpecialActionNames { get => CreatureStats.SpecialActionNames; }
    public Vector3 ViewCenter { get => transform.position + _spriteOffset; }
    public Element PrimaryElement { get => CreatureStats.Species.PrimaryElement; }
    public Element SecondaryElement { get => CreatureStats.Species.SecondaryElement; }
    public bool InBattle { get; set; }
    public Vector3Int Position { get; set; }
    public int CurrentHP { get; private set; }
    public int CurrentInitiative { get; private set; }
    public bool HasMoved { get; set; }
    public bool HasBasicAttacked { get; set; }

    GameObject _healthBarObject;
    GameObject _healthBarBackgroundObject;
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
        Position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        CurrentHP = CreatureStats.MaximumHP;
        CurrentInitiative = 0;
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
        _spriteRenderer.sprite = CreatureStats.Species.BaseSprite;
        _spriteRenderer.color = CreatureStats.Species.BaseColor;
        // Create health bar.
        _healthBarObject = CreateHealthBarObject("Health Bar", Color.green, false);
        _healthBarBackgroundObject = CreateHealthBarObject("Health Bar Background", Color.black, true);
    }

    GameObject CreateHealthBarObject(string name, Color color, bool isBackground)
    {
        GameObject go = new GameObject { name = name };
        float z = isBackground ? -8 : -8.25f;
        go.transform.position += new Vector3(_spriteOffset.x, 2 * _spriteOffset.y + 0.25f, z);
        if (isBackground)
        {
            go.transform.localScale = new Vector3(1.125f, 0.25f, 1);
        }
        else
        {
            go.transform.localScale = new Vector3(1, 0.125f, 1);
        }
        go.transform.parent = transform;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = AssetLibrary.CreateSquareSprite(4, color);
        return go;
    }

    void UpdateHealthBar()
    {
        if (CurrentHP <= 0)
        {
            _healthBarObject.SetActive(false);
            _healthBarBackgroundObject.SetActive(false);
            return;
        }
        _healthBarObject.SetActive(true);
        _healthBarBackgroundObject.SetActive(true);
        float percent = Math.Min(1, (float)CurrentHP / CreatureStats.MaximumHP);
        _healthBarObject.transform.localScale = new Vector3(percent, 0.125f, 1);
        _healthBarObject.transform.position = _healthBarBackgroundObject.transform.position
            + new Vector3((percent - 1) / 2, 0, -0.25f);
    }

    public void SetVisible(bool isVisible)
    {
        _spriteRenderer.enabled = isVisible;
    }

    public void RemoveFromBattle()
    {
        InBattle = false;
        SetVisible(false);
        CurrentInitiative = int.MinValue;
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
            int thisSpeed = CreatureStats.GetStatTotal(Stat.Speed);
            int otherSpeed = other.CreatureStats.GetStatTotal(Stat.Speed);
            if (thisSpeed > otherSpeed)
            {
                return -1;
            }
            else if (thisSpeed == otherSpeed)
            {
                int thisSpeedMod = CreatureStats.GetStatIndividual(Stat.Speed);
                int otherSpeedMod = other.CreatureStats.GetStatIndividual(Stat.Speed);
                if (thisSpeedMod > otherSpeedMod)
                {
                    return -1;
                }
                else if (thisSpeedMod == otherSpeedMod)
                {
                    return UnitID < other.UnitID ? -1 : 1;
                }
            }
        }
        return 1;
    }

    public void IncrementInitiative()
    {
        if (InBattle)
        {
            CurrentInitiative += TurnOrder.INITIATIVE_INCREMENT + CreatureStats.GetStatTotal(Stat.Speed);
        }
    }

    public void ConsumeInitiative()
    {
        CurrentInitiative -= TurnOrder.INITIATIVE_THRESHOLD;
    }

    public void MoveTo(int x, int y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
        Position = new Vector3Int(x, y, 0);
    }

    public void TakeDamage(Element element, int amount, bool isPercentage)
    {
        // Ignore negative or zero damage.
        if (amount <= 0)
        {
            return;
        }
        // Convert percentage to amount of damage.
        if (isPercentage)
        {
            amount = (int)Math.Round(CreatureStats.MaximumHP * amount / 100f);
        }
        // Elemental Resistances
        if (element == PrimaryElement)
        {
            if (element != Element.NoElement)
            {
                amount = (int)Math.Round(amount * 0.5f);
            }
        }
        else if (element == SecondaryElement)
        {
            if (element != Element.NoElement)
            {
                amount = (int)Math.Round(amount * 0.75f);
            }
        }
        // Reduce the unit's hit points.
        CurrentHP -= amount;
        UpdateHealthBar();
    }

    public void Heal(Element element, int amount, bool isPercentage)
    {
        // Ignore negative or zero healing.
        if (amount <= 0)
        {
            return;
        }
        // Convert percentage to amount of damage.
        if (isPercentage)
        {
            amount = (int)Math.Round(CreatureStats.MaximumHP * amount / 100f);
        }
        // Elemental Affinities
        if (element == PrimaryElement)
        {
            if (element != Element.NoElement)
            {
                amount = (int)Math.Round(amount * 1.5f);
            }
        }
        else if (element == SecondaryElement)
        {
            if (element != Element.NoElement)
            {
                amount = (int)Math.Round(amount * 1.25f);
            }
        }
        // Increase the unit's hit points.
        CurrentHP += amount;
        UpdateHealthBar();
    }
}
