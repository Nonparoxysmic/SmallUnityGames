using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int direction;

    [SerializeField] float speed;

    static readonly int[,] directions = new int[,] { { 5, 6, 7 }, { 4, -1, 0 }, { 3, 2, 1 } };

    Vector3 moveInput;

    void Update()
    {
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            direction = Direction(moveInput.x, moveInput.y);
            moveInput /= moveInput.magnitude;
            transform.position += speed * Time.deltaTime * moveInput;
        }
    }

    static int Direction(float x, float y)
    {
        int col = (int)x + 1;
        int row = (int)y + 1;
        return directions[col, row];
    }
}
