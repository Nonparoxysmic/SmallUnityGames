using UnityEngine;

public class GameMaster : MonoBehaviour
{
    static readonly int[] actionTimes = new int[] { 15, 30, 30, 30, 15, 30, 30, 30 };

    public int ToolbarSize { get => 4; }

    [SerializeField] Player player;
    [SerializeField] Toolbar toolbar;
    [SerializeField] ProgressMeter progressMeter;

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
        if (progressMeter is null)
        {
            this.Error("Progress Meter reference not set in Inspector.");
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

        int playerX = Mathf.RoundToInt(player.ColliderPosition().x);
        int playerY = Mathf.RoundToInt(player.ColliderPosition().y);
        int currentBackgroundTile = worldManager.GetBackgroundTileIndex(playerX, playerY);
        int currentObjectTile = worldManager.GetObjectTileIndex(playerX, playerY);
        if (player.onRaft)
        {
            if (currentObjectTile.EqualsOneOf(4, 5, 6)
                || currentBackgroundTile.EqualsOneOf(1, 2, 3, 8))
            {
                worldManager.DropRaft(playerX, playerY);
                (int, int) nearestLand = worldManager.NearestOpenLand(playerX, playerY);
                player.Teleport(nearestLand.Item1 - playerX, nearestLand.Item2 - playerY);
                player.Collision(true);
                player.onRaft = false;
            }
        }
        else if(currentBackgroundTile == 8)
        {
            player.Collision(false);
            player.onRaft = true;
            worldManager.SetBackgroundTile(playerX, playerY, 0, true);
        }
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
            progressMeter.Show(false);
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
                actionProgress = -10;
                progressMeter.Show(false);
            }
            else if (actionProgress < 0)
            {
                progressMeter.Show(false);
            }
            else
            {
                progressMeter.SetHorzScale((float)(completeTime - actionProgress) / completeTime);
                progressMeter.Show(true);
            }
        }
    }

    public void ChangeTool(int option)
    {
        toolbar.SetCurrent(option);
    }

    public void ChangeToolDelta(int delta)
    {
        toolbar.SetCurrentDelta(delta);
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
                return targetObjectTile == 5 || targetObjectTile == 6 || targetBackgroundTile == 8;
            case 1:
                // Dig
                if (targetObjectTile < 0)
                {
                    return targetBackgroundTile.EqualsOneOf(1, 2, 3);
                }
                if (targetObjectTile == 4) { return true; }
                return false;
            case 2:
                // Break
                return targetObjectTile == 4 || targetBackgroundTile == 8;
            case 3:
                // Carve
                return targetObjectTile == 5;
            case 4:
                // Drop
                return targetObjectTile < 0 && targetBackgroundTile != 8;
            case 5:
                // Un-Dig
                if (player.inventory[toolbar.current] == 4)
                {
                    return targetObjectTile < 0 && targetBackgroundTile.EqualsOneOf(1, 2, 3);
                }
                return targetBackgroundTile == 0 || worldManager.IsHole(x, y);
            case 6:
                // Build
                return targetObjectTile < 0 && targetBackgroundTile == 0;
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
                if (targetBackgroundTile == 8)
                {
                    Dig(x, y);
                }
                else
                {
                    PickUpObject(x, y);
                }
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
                // Break
                if (targetObjectTile == 4)
                {
                    ChangeObject(x, y, 7);
                    PickUpObject(x, y);
                }
                if (targetBackgroundTile == 8)
                {
                    worldManager.SetBackgroundTile(x, y, 0, true);
                }
                return;
            case 3:
                // Carve
                if (targetObjectTile == 5)
                {
                    ChangeObject(x, y, 6);
                }
                return;
            case 4:
                // Drop
                if (player.inventory[toolbar.current] == 8)
                {
                    if (targetBackgroundTile == 0)
                    {
                        UnDig(x, y);
                    }
                }
                else
                {
                    DropObject(x, y);
                }
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
                // Build
                player.inventory[toolbar.current] = 8;
                UnDig(x, y);
                return;
            case 7:
            default:
                return;
        }
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

    void ChangeObject(int x, int y, int index)
    {
        worldManager.SetObjectTile(x, y, index);
        targetObjectTile = index;
    }
}
