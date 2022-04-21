using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    bool testAction;
    Vector3Int directionalInput;

    void Update()
    {
        testAction = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q);
    }

    void FixedUpdate()
    {
        directionalInput.x = (int)Input.GetAxisRaw("Horizontal");
        directionalInput.y = (int)Input.GetAxisRaw("Vertical");
        player.isStrafing = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        player.PlayerMovement(directionalInput);
        player.TestAction(testAction);
    }
}
