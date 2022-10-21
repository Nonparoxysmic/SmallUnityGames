using UnityEngine;

public class World : MonoBehaviour
{
    WorldLoader _worldLoader;

    public void CreateWorld(WorldLoader worldLoader)
    {
        _worldLoader = worldLoader;

        // TODO: Get world data from WorldLoader and create world GameObjects. (E.g. Tilemaps and Sprites)
    }
}
