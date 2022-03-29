using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static int subunitsPerUnit = 16;
    static float subunitSize;

    [SerializeField] float speed;

    Vector3 move;
    Vector3 moveInput;

    void Start()
    {
        subunitSize = 1.0f / subunitsPerUnit;
    }

    void Update()
    {
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            moveInput /= moveInput.magnitude;
            move = speed * Time.deltaTime * moveInput;
            move.x = NonzeroFloor(move.x, subunitSize);
            move.y = NonzeroFloor(move.y, subunitSize);
            transform.position += move;
        }
    }

    float NonzeroFloor(float number, float significance)
    {
        if (number == 0) { return 0; }
        if (Mathf.Sign(number) != Mathf.Sign(significance))
        {
            significance *= -1;
        }
        return significance * Mathf.Max(Mathf.Floor(number / significance), 1);
    }
}
