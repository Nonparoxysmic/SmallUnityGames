using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    static readonly int[,] directions = new int[,] { { 1, 2, 3 }, { 0, -1, 4 }, { 7, 6, 5 } };

    public int direction;
    public Vector3Int facing;

    [SerializeField] float speed;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Transform targetTest;

    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    internal bool IsStrafing { get; set; }

    void Start()
    {
        facing = new Vector3Int(0, -1, 0);
    }

    internal void PlayerMovement(Vector3Int directionalInput)
    {
        int inputDirection = Direction(directionalInput.x, directionalInput.y);
        if (inputDirection >= 0)
        {
            if (!IsStrafing)
            {
                facing = new Vector3Int(directionalInput.x, directionalInput.y, 0);
                playerSpriteRenderer.sprite = playerSprites[inputDirection];
            }
            transform.position += speed * Time.fixedDeltaTime / directionalInput.magnitude
                * (Vector3)directionalInput;
            targetTest.position = new Vector3(Mathf.FloorToInt(transform.position.x) + facing.x + 0.5f,
                Mathf.FloorToInt(transform.position.y) + facing.y + 0.5f, targetTest.position.z);
        }
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
