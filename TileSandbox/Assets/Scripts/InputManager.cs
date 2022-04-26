using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Toolbar toolbar;

    bool mouseMoved;
    Vector3 previousMousePosition;
    Vector3Int directionalInput;

    void Update()
    {
        if (Input.mousePosition != previousMousePosition)
        {
            mouseMoved = true;
            previousMousePosition = Input.mousePosition;
        }
        for (int i = 1; i <= toolbar.numberOfOptions; i++)
        {
            int key = i + 48;
            if (Input.GetKeyDown((KeyCode)key))
            {
                toolbar.SetOption(i);
                player.ChangeTool(i);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        directionalInput.x = (int)Input.GetAxisRaw("Horizontal");
        directionalInput.y = (int)Input.GetAxisRaw("Vertical");
        player.isStrafing = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (mouseMoved)
        {
            player.PlayerMovement(directionalInput, Input.mousePosition);
            mouseMoved = false;
        }
        else player.PlayerMovement(directionalInput);
        bool testAction = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Q);
        player.TestAction(testAction);
    }
}
