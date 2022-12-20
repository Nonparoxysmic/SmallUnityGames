using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteBook", menuName = "TacticalCreatureBattle/SpriteBook")]
public class SpriteBook : ScriptableObject
{
    public Sprite[] CreatureSpritesSmall;
    public Sprite[] CreatureSpritesMedium;
    public Sprite[] CreatureSpritesLarge;
}
