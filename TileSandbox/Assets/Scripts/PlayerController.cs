using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public readonly Vector3Int[] targetingVectors = new Vector3Int[]
    {
        new Vector3Int( 0, -1,  0),
        new Vector3Int(-1, -1,  0),
        new Vector3Int(-1,  0,  0),
        new Vector3Int(-1,  1,  0),
        new Vector3Int( 0,  1,  0),
        new Vector3Int( 1,  1,  0),
        new Vector3Int( 1,  0,  0),
        new Vector3Int( 1, -1,  0)
    };

    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Transform target;
    [SerializeField] SpriteRenderer workingSprite;

    [SerializeField] float normalSpeed;
    [SerializeField] int direction;
    public bool isStrafing;
    [SerializeField] int diagonalFrames;
    [SerializeField] int addingFrames;
    [SerializeField] int removingFrames;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;
    bool testTool = true;

    int diagonalLockCountdown;
    int lockedDirection;
    int previousInputDirection;
    int workingTargetClock;
    (int, int) workingTarget;
    Transform workingSpriteTransform;
    [HideInInspector] public Vector3Int targetingVector;
    [HideInInspector] public Vector3Int targetTile;

    void Start()
    {
        targetingVector = new Vector3Int(0, -1, 0);
        targetTile.x = Mathf.FloorToInt(transform.position.x) + targetingVector.x;
        targetTile.y = Mathf.FloorToInt(transform.position.y) + targetingVector.y;
        workingSpriteTransform = workingSprite.gameObject.transform;
    }

    internal void PlayerMovement(int inputDirection)
    {
        if (inputDirection >= 0)
        {
            float distance = normalSpeed * Time.fixedDeltaTime;

            // If input direction just changed from a diagonal to an adjacent orthogonal...
            if (inputDirection.EqualsOneOf(0, 2, 4, 6)
                && Utilities.DirectionsAreAdjacent(inputDirection, previousInputDirection))
            {
                // Lock the diagonal orientation for a few frames.
                diagonalLockCountdown = diagonalFrames;
                lockedDirection = previousInputDirection;
            }
            if (diagonalLockCountdown > 0)
            {
                if (inputDirection == lockedDirection
                    || Utilities.DirectionsAreAdjacent(inputDirection, lockedDirection))
                {
                    direction = lockedDirection;
                }
                else
                {
                    direction = inputDirection;
                    diagonalLockCountdown = 0;
                }
            }
            else
            {
                direction = inputDirection;
            }

            // If strafing, reduce speed.
            // If not, update facing direction.
            if (isStrafing)
            {
                distance /= 2;
            }
            else
            {
                targetingVector = targetingVectors[direction];
                playerSpriteRenderer.sprite = playerSprites[direction];
            }

            // Move.
            Vector3 v = targetingVectors[direction];
            transform.position += distance / v.magnitude * v;
        }
        if (diagonalLockCountdown > 0) { diagonalLockCountdown--; }
        previousInputDirection = inputDirection;
    }

    internal void ChangeTool(int option)
    {
        testTool = option == 1;
    }

    internal void TestAction(bool isActive)
    {
        float bar = 0;
        var currentTile = collisionTilemap.GetTile(targetTile);
        if ((currentTile == squareTile && !testTool)
            || (currentTile == null && testTool))
        {
            // Wrong tool
            workingTargetClock = 0;
        }
        else if (!isActive)
        {
            workingTargetClock = 0;
        }
        else if (workingTarget.Item1 != targetTile.x || workingTarget.Item2 != targetTile.y)
        {
            workingTarget = (targetTile.x, targetTile.y);
            workingTargetClock = 0;
            bar = 3;
        }
        else
        {
            workingTargetClock++;
            int workingFrames;
            if (currentTile == squareTile)
            {
                workingFrames = removingFrames;
            }
            else
            {
                workingFrames = addingFrames;
            }
            bar = 3.0f * (workingFrames - workingTargetClock) / workingFrames;
            if (workingTargetClock >= workingFrames)
            {
                TestAction2();
                workingTargetClock = 0;
            }
        }
        workingSpriteTransform.localScale = new Vector3(bar, 0.125f, 1);
    }

    internal void TestAction2()
    {
        if (collisionTilemap.GetTile(targetTile) == squareTile)
        {
            collisionTilemap.SetTile(targetTile, null);
        }
        else
        {
            collisionTilemap.SetTile(targetTile, squareTile);
        }
    }
}
