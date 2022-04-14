using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerController player;

    bool testAction;
    Vector3Int directionalInput;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testAction = true;
        }
    }

    void FixedUpdate()
    {
        directionalInput.x = (int)Input.GetAxisRaw("Horizontal");
        directionalInput.y = (int)Input.GetAxisRaw("Vertical");
        player.isStrafing = Input.GetKey(KeyCode.LeftShift);
        player.PlayerMovement(directionalInput);
        if (testAction)
        {
            player.TestAction();
            testAction = false;
        }
    }
}
