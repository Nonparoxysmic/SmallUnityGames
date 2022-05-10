using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] Player player;

    void Start()
    {
        if (player is null)
        {
            Utilities.ComponentError(this, "Player GameObject not set in Inspector.");
            return;
        }
    }

    public Vector3 PlayerPosition()
    {
        return player.transform.position;
    }

    public void OnDirectionalInput(int direction)
    {
        player.Move(direction);
        transform.position = PlayerPosition();
    }
}
