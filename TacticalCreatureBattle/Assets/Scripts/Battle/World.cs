using UnityEngine;

public class World : MonoBehaviour
{
    // This component creates and manages all of the GameObjects (such as Tilemaps and Sprites)
    // that represent the battle map and the creatures.

    Map _map;

    public void CreateWorld(WorldLoader worldLoader, CreatureData creatureData)
    {
        _map = worldLoader.Current;
        if (_map == null)
        {
            this.Error("Current world is null.");
            return;
        }
        // TODO: Create world GameObjects. (E.g. Tilemaps and Sprites)
        // TODO: Create creature sprites.
    }
}
