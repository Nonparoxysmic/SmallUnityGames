using UnityEngine;

public class GameMaster : MonoBehaviour
{
    static readonly int[] actionTimes = new int[] { 10, 30, 30, 30, 10, 10, 10, 10 };

    public int ToolbarSize { get => 4; }

    [SerializeField] Player player;
    [SerializeField] Toolbar toolbar;

    [HideInInspector] public bool playerIsMoving;
    public int actionProgress;

    WorldManager worldManager;

    (int, int) currentTarget;
    int previousTool;
    (int, int) previousTarget;
    int targetBackgroundTile;
    int targetObjectTile;

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
        currentTarget = ((int)position.x, (int)position.y);
        if (currentTarget != previousTarget || toolbar.current != previousTool)
        {
            previousTarget = currentTarget;
            previousTool = toolbar.current;
            actionProgress = -1;
            return;
        }
        actionProgress++;

        int x = currentTarget.Item1;
        int y = currentTarget.Item2;
        targetBackgroundTile = worldManager.GetBackgroundTileIndex(x, y);
        targetObjectTile = worldManager.GetObjectTileIndex(x, y);
        if (IsValidTool(x, y, out int completeTime, out int actionNumber))
        {
            if (actionProgress >= completeTime)
            {
                DoAction(actionNumber, x, y);
                actionProgress = -5;
            }
        }
    }

    public void ChangeTool(int option)
    {
        toolbar.SetCurrent(option);
    }

    bool IsValidTool(int x, int y, out int completeTime, out int actionNumber)
    {
        actionNumber = toolbar.current;
        int item = player.inventory[toolbar.current];
        if (item > 0) { actionNumber += 4; }
        completeTime = actionTimes[actionNumber];

        switch (actionNumber)
        {
            case 0:
                // Pick Up
                return targetObjectTile == 5;
            case 1:
                // Dig
                if (targetObjectTile < 0)
                {
                    return targetBackgroundTile.EqualsOneOf(1, 2, 3);
                }
                if (targetObjectTile == 4) { return true; }
                return false;
            case 2:
            case 3:
                if (targetObjectTile < 0) return false;
                return true;
            case 4:
                // Drop
                return targetObjectTile < 0;
            case 5:
                // Un-Dig
                if (player.inventory[toolbar.current] == 4)
                {
                    return targetObjectTile < 0 && targetBackgroundTile.EqualsOneOf(1, 2, 3);
                }
                return targetBackgroundTile == 0 || worldManager.IsHole(x, y);
            case 6:
            case 7:
            default:
                return false;
        }
    }

    void DoAction(int actionNumber, int x, int y)
    {
        switch (actionNumber)
        {
            case 0:
                // Pick Up
                PickUpObject(x, y);
                return;
            case 1:
                // Dig
                if (targetObjectTile < 0)
                {
                    Dig(x, y);
                }
                if (targetObjectTile == 4)
                {
                    PickUpObject(x, y);
                }
                return;
            case 2:
            case 3:
                return;
            case 4:
                // Drop
                DropObject(x, y);
                return;
            case 5:
                // Un-Dig
                if (player.inventory[toolbar.current] == 4)
                {
                    DropObject(x, y);
                }
                else
                {
                    UnDig(x, y);
                }
                return;
            case 6:
            case 7:
            default:
                return;
        }

        void PickUpObject(int x, int y)
        {
            player.inventory[toolbar.current] = targetObjectTile;
            toolbar.SetIcon(toolbar.current, worldManager.GetTileSprite(targetObjectTile));
            if (targetBackgroundTile == 0)
            {
                worldManager.ClearObjectTile(x, y);
            }
            else
            {
                worldManager.ClearObjectTile(x, y, false);
            }
        }

        void DropObject(int x, int y)
        {
            worldManager.SetObjectTile(x, y, player.inventory[toolbar.current], true);
            player.inventory[toolbar.current] = 0;
            toolbar.ResetIcon(toolbar.current);
        }

        void Dig(int x, int y)
        {
            if (targetBackgroundTile == 2) { targetBackgroundTile = 3; }
            player.inventory[toolbar.current] = targetBackgroundTile;
            toolbar.SetIcon(toolbar.current, worldManager.GetTileSprite(targetBackgroundTile));
            worldManager.AddHole(x, y);
        }

        void UnDig(int x, int y)
        {
            if (!worldManager.RemoveHole(x, y, player.inventory[toolbar.current]))
            {
                worldManager.SetBackgroundTile(x, y, player.inventory[toolbar.current], false);
            }
            player.inventory[toolbar.current] = 0;
            toolbar.ResetIcon(toolbar.current);
        }
    }
}
