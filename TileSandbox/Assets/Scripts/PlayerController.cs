using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    static readonly int[,] directions = new int[,] { { 1, 2, 3 }, { 0, -99, 4 }, { 7, 6, 5 } };
    static readonly Vector3Int[] targetingVectors = new Vector3Int[]
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
    static readonly Vector3 tileOffset = new Vector3(0.5f, 0.5f);

    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Transform target;
    [SerializeField] SpriteRenderer workingSprite;

    [SerializeField] float normalSpeed;
    [SerializeField] int direction;
    public bool isStrafing;
    [SerializeField] int diagonalFrames;
    [SerializeField] int workingFrames;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    int diagonalLockCountdown;
    int lockedDirection;
    int previousInputDirection;
    int workingTargetClock;
    (int, int) workingTarget;
    Transform workingSpriteTransform;
    Vector3Int targetingVector;
    Vector3Int targetTile;

    void Start()
    {
        targetingVector = new Vector3Int(0, -1, 0);
        targetTile.x = Mathf.FloorToInt(transform.position.x) + targetingVector.x;
        targetTile.y = Mathf.FloorToInt(transform.position.y) + targetingVector.y;
        workingSpriteTransform = workingSprite.gameObject.transform;
    }

    internal void PlayerMovement(Vector3Int directionalInput)
    {
        int inputDirection = Direction(directionalInput.x, directionalInput.y);
        if (inputDirection >= 0)
        {
            // If input direction just changed from a diagonal to an adjacent orthogonal...
            if (inputDirection.EqualsOneOf(0, 2, 4, 6)
                && DirectionsAreAdjacent(inputDirection, previousInputDirection))
            {
                diagonalLockCountdown = diagonalFrames;
                lockedDirection = previousInputDirection;
            }

            float distance = normalSpeed * Time.fixedDeltaTime;
            if (diagonalLockCountdown > 0)
            {
                if (inputDirection == lockedDirection
                    || DirectionsAreAdjacent(inputDirection, lockedDirection))
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
            if (isStrafing)
            {
                distance /= 2;
            }
            else
            {
                targetingVector = targetingVectors[direction];
                playerSpriteRenderer.sprite = playerSprites[direction];
            }
            Vector3 v = targetingVectors[direction];
            transform.position += distance / v.magnitude * v;
            targetTile.x = Mathf.FloorToInt(transform.position.x) + targetingVector.x;
            targetTile.y = Mathf.FloorToInt(transform.position.y) + targetingVector.y;
            target.position = targetTile + tileOffset;
            if (diagonalLockCountdown > 0) { diagonalLockCountdown--; }
        }
        previousInputDirection = inputDirection;
    }

    internal void TestAction(bool isActive)
    {
        float bar = 0;
        if (!isActive)
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
            if (workingTargetClock >= workingFrames)
            {
                TestAction2();
                workingTargetClock = 0;
            }
            bar = 3.0f * (workingFrames - workingTargetClock) / workingFrames;
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

    static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }

    static bool DirectionsAreAdjacent(int a, int b)
    {
        return Math.Abs(a - b) == 1 || (a == 0 && b == 7) || (b == 0 && a == 7);
    }
}
