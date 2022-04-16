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

    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Transform target;

    [SerializeField] float speed;
    [SerializeField] int direction;
    public bool isStrafing;

    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    int previousInputDirection;
    Vector3Int targetingVector;

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
                && (Math.Abs(inputDirection - previousInputDirection) == 1
                || (inputDirection == 0 && previousInputDirection == 7)))
            {
                // TODO: Make diagonal aiming feel better.
            }

            if (isStrafing)
            {
                transform.position += 0.5f * speed * Time.fixedDeltaTime / directionalInput.magnitude
                * (Vector3)directionalInput;
            }
            else
            {
                direction = inputDirection;
                targetingVector = targetingVectors[direction];
                playerSpriteRenderer.sprite = playerSprites[direction];
                transform.position += speed * Time.fixedDeltaTime / directionalInput.magnitude
                    * (Vector3)directionalInput;
            }
            target.position = new Vector3(Mathf.FloorToInt(transform.position.x) + targetingVector.x + 0.5f,
                Mathf.FloorToInt(transform.position.y) + targetingVector.y + 0.5f, target.position.z);
        }
        previousInputDirection = inputDirection;
    }

    internal void TestAction()
    {
        Vector3Int target = new Vector3Int(Mathf.FloorToInt(transform.position.x) + targetingVector.x,
            Mathf.FloorToInt(transform.position.y) + targetingVector.y, 0);
        if (collisionTilemap.GetTile(target) == squareTile)
        {
            collisionTilemap.SetTile(target, null);
        }
        else
        {
            collisionTilemap.SetTile(target, squareTile);
        }
    }

    static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }
}
