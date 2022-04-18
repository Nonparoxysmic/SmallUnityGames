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

    [SerializeField] float normalSpeed;
    [SerializeField] int direction;
    public bool isStrafing;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    int previousInputDirection;
    Vector3Int targetingVector;
    Vector3Int targetTile;

    void Start()
    {
        targetingVector = new Vector3Int(0, -1, 0);
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
                // TODO: Make diagonal aiming feel better.
            }

            float currentSpeed = normalSpeed * Time.fixedDeltaTime / directionalInput.magnitude;
            if (isStrafing)
            {
                currentSpeed /= 2;
            }
            else
            {
                direction = inputDirection;
                targetingVector = targetingVectors[direction];
                playerSpriteRenderer.sprite = playerSprites[direction];
            }
            transform.position += currentSpeed * (Vector3)directionalInput;
            targetTile.x = Mathf.FloorToInt(transform.position.x) + targetingVector.x;
            targetTile.y = Mathf.FloorToInt(transform.position.y) + targetingVector.y;
            target.position = targetTile + tileOffset;
        }
        previousInputDirection = inputDirection;
    }

    internal void TestAction()
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
