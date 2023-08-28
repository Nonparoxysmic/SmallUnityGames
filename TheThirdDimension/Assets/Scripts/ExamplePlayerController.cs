using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ExamplePlayerController : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float RotateSpeed = 120;

    CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * RotateSpeed * Time.deltaTime, 0);
        _characterController.SimpleMove(Input.GetAxis("Vertical") * MoveSpeed * transform.forward);
    }
}
