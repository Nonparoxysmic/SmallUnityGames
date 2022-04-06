using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int direction;
    public Vector2Int facing;

    [SerializeField] float speed;
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] Transform targetTest;

    static readonly int[,] directions = new int[,] { { 5, 6, 7 }, { 4, -1, 0 }, { 3, 2, 1 } };

    Vector3 moveInput;

    void Update()
    {
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            direction = Direction(moveInput.x, moveInput.y);
            playerSpriteRenderer.sprite = playerSprites[direction];
            transform.position += speed * Time.deltaTime * (moveInput / moveInput.magnitude);
            facing = new Vector2Int(Mathf.FloorToInt(transform.position.x) + (int)moveInput.x,
                Mathf.FloorToInt(transform.position.y) + (int)moveInput.y);
            targetTest.position = new Vector3(facing.x + 0.5f, facing.y + 0.5f, targetTest.position.z);
        }
    }

    static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }
}
