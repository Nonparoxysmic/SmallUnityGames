using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    Vector3 moveInput;

    void Update()
    {
        moveInput.x = (int)Input.GetAxisRaw("Horizontal");
        moveInput.y = (int)Input.GetAxisRaw("Vertical");
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            moveInput /= moveInput.magnitude;
            transform.position += speed * Time.deltaTime * moveInput;
        }
    }
}
