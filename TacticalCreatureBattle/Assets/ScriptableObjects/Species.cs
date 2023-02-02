using UnityEngine;

[CreateAssetMenu(fileName = "New Species", menuName = "TacticalCreatureBattle/Species")]
public class Species : ScriptableObject
{
    public Size Size;
    public Sprite BaseSprite;
    public Color BaseColor;
}
