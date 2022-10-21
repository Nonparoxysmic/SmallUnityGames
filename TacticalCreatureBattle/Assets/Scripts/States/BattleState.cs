public abstract class BattleState : State
{
    public BattleSceneManager BattleSceneManager { get => BattleSceneManager.Instance; }
    public CreatureData CreatureData { get => BattleSceneManager.Instance.CreatureData; }
    public World World { get => BattleSceneManager.Instance.World; }
}
