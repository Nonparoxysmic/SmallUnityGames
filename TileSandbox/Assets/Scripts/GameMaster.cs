using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] Player player;

    public bool playerIsMoving;

    void Start()
    {
        if (player is null)
        {
            this.Error("Player GameObject not set in Inspector.");
            return;
        }
        playerIsMoving = true;
    }

    public Vector3 PlayerFacingPosition()
    {
        return player.ColliderPosition() + Utilities.DirectionVector(player.facingDirection);
    }

    public Vector3 PlayerFacingPosition(int direction)
    {
        if (direction < 0 || direction >= 8)
        {
            return PlayerFacingPosition();
        }
        return player.ColliderPosition() + Utilities.DirectionVector(direction);
    }

    public void SetPlayerFacingDirection(int direction)
    {
        if (direction < 0 || direction >= 8) { return; }

        if (!player.isStrafing)
        {
            player.facingDirection = direction;
        }
    }

    public void SetPlayerStrafing(bool strafeKeyDown)
    {
        player.isStrafing = strafeKeyDown;
    }

    public void OnDirectionalInput(int direction)
    {
        if (direction < 0 || direction >= 8) { return; }

        player.Move(direction);
        playerIsMoving = true;
    }
}
