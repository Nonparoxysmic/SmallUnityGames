using UnityEngine;

[CreateAssetMenu(fileName = "New Species", menuName = "TacticalCreatureBattle/Species")]
public class Species : ScriptableObject
{
    public string DisplayName;

    public Size Size;
    public Sprite BaseSprite;
    public Color BaseColor;
    public bool CanFly;

    public int Health = 5; 
    public int Strength = 5;
    public int Magic = 5;
    public int Defense = 5;
    public int Speed = 5;

    public Element PrimaryElement = Element.NoElement;
    public Element SecondaryElement = Element.NoElement;

    public LearnableAction[] LearnableActions;
}
