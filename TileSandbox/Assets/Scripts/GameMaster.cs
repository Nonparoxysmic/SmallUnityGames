using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Toolbar toolbar;

    public bool playerIsMoving;
    public int toolbarSize;

    void Start()
    {
        if (player is null)
        {
            this.Error("Player GameObject not set in Inspector.");
            return;
        }
        if (toolbar is null)
        {
            this.Error("Toolbar reference not set in Inspector.");
            return;
        }
        if (toolbarSize < 1 || toolbarSize > 9)
        {
            this.Error("Toolbar size must be in the range [1, 9].");
            return;
        }
        toolbar.Create(toolbarSize);
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

    public void OnActionKey(Vector3 position)
    {

    }

    public void ChangeTool(int option)
    {
        if (option < 0 || option >= toolbar.Size) { return; }

        toolbar.SetCurrent(option);
    }
}
