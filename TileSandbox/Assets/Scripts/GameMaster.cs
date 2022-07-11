using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public int ToolbarSize { get => 4; }

    [SerializeField] Player player;
    [SerializeField] Toolbar toolbar;

    [HideInInspector] public bool playerIsMoving;
    public int actionProgress;

    WorldManager worldManager;

    int previousTool;
    (int, int) previousTarget;

    void Start()
    {
        worldManager = GetComponent<WorldManager>();
        if (worldManager is null)
        {
            this.Error("Missing or unavailable World Manager.");
            return;
        }
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
        toolbar.Create(ToolbarSize);
        playerIsMoving = true;
    }

    public Vector3 PlayerPosition()
    {
        return player.ColliderPosition();
    }

    public Vector3 PlayerFacingPosition()
    {
        return player.ColliderPosition() + Utilities.DirectionVector(player.facingDirection);
    }

    public Vector3 PlayerFacingPosition(int direction)
    {
        if (!direction.InRange(0, 8))
        {
            return PlayerFacingPosition();
        }
        return player.ColliderPosition() + Utilities.DirectionVector(direction);
    }

    public void SetPlayerFacingDirection(int direction)
    {
        if (!direction.InRange(0, 8)) { return; }

        if (!player.isStrafing)
        {
            player.facingDirection = direction;
        }
    }

    public void SetPlayerStrafing(bool strafeKeyDown)
    {
        player.isStrafing = strafeKeyDown;
    }

    public bool GetPlayerStrafing()
    {
        return player.isStrafing;
    }

    public void OnDirectionalInput(int direction)
    {
        if (!direction.InRange(0, 8)) { return; }

        player.Move(direction);
        playerIsMoving = true;
    }

    public void OnActionKey(bool actionKeyPressed, Vector3 position)
    {
        if (!actionKeyPressed)
        {
            previousTool = -1;
        }
        (int, int) currentTarget = ((int)position.x, (int)position.y);
        if (currentTarget != previousTarget || toolbar.current != previousTool)
        {
            previousTarget = currentTarget;
            previousTool = toolbar.current;
            actionProgress = -1;
            return;
        }
        actionProgress++;

        int targetBackgroundTile = worldManager
            .GetBackgroundTileIndex(currentTarget.Item1, currentTarget.Item2);
        int targetObjectTile = worldManager
            .GetObjectTileIndex(currentTarget.Item1, currentTarget.Item2);

        // TODO: Implement actions
        if (actionProgress > 30)
        {
            Debug.Log("Action completed.");
            actionProgress = -5;
        }
    }

    public void ChangeTool(int option)
    {
        toolbar.SetCurrent(option);
    }
}
