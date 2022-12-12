using UnityEngine;

[CreateAssetMenu(fileName ="New CreatureStats", menuName = "TacticalCreatureBattle/CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string CreatureName;
    public int MaximumHP;
}
