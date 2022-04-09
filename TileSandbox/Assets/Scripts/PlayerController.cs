using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public int direction;
    public Vector3Int facing;

    [SerializeField] float speed;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Transform targetTest;

    [SerializeField] Tilemap collisionTilemap;
    [SerializeField] Tile squareTile;

    static readonly int[,] directions = new int[,] { { 5, 6, 7 }, { 4, -1, 0 }, { 3, 2, 1 } };

    Vector3 moveInput;

    void Start()
    {
        facing = new Vector3Int(0, 1, 0);
    }

    void Update()
    {
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                direction = Direction(moveInput.x, moveInput.y);
                playerSpriteRenderer.sprite = playerSprites[direction];
            }
            transform.position += speed * Time.deltaTime / moveInput.magnitude * moveInput;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                facing = new Vector3Int((int)moveInput.x, (int)moveInput.y, 0);
            }
            targetTest.position = new Vector3(Mathf.FloorToInt(transform.position.x) + facing.x + 0.5f, Mathf.FloorToInt(transform.position.y) + facing.y + 0.5f, targetTest.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
    }

    static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }
}
