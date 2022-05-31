using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldManager : MonoBehaviour
{
    [SerializeField] Tilemap backgroundTilemap;
    [SerializeField] Tilemap collisionTilemap;

    void Start()
    {
        if (backgroundTilemap is null)
        {
            this.Error("Background Tilemap reference not set in Inspector.");
            return;
        }
        if (collisionTilemap is null)
        {
            this.Error("Collision Tilemap reference not set in Inspector.");
            return;
        }
    }
}
