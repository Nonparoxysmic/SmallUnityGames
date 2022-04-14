using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    static readonly int[,] directions = new int[,] { { 1, 2, 3 }, { 0, -99, 4 }, { 7, 6, 5 } };

    [SerializeField] int direction;
    public bool isStrafing;

    [SerializeField] Vector3Int facing;
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Transform targetTest;
    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    int previousInputDirection;

    void Start()
    {
        facing = new Vector3Int(0, -1, 0);
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
                facing = new Vector3Int(directionalInput.x, directionalInput.y, 0);
                playerSpriteRenderer.sprite = playerSprites[direction];
                transform.position += speed * Time.fixedDeltaTime / directionalInput.magnitude
                    * (Vector3)directionalInput;
            }
            targetTest.position = new Vector3(Mathf.FloorToInt(transform.position.x) + facing.x + 0.5f,
                Mathf.FloorToInt(transform.position.y) + facing.y + 0.5f, targetTest.position.z);
        }
        previousInputDirection = inputDirection;
    }

    internal void TestAction()
    {
        Vector3Int target = new Vector3Int(Mathf.FloorToInt(transform.position.x) + facing.x,
            Mathf.FloorToInt(transform.position.y) + facing.y, 0);
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
