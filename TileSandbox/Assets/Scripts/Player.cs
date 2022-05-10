using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float normalSpeed;

    Collider2D playerCollider;

    void Start()
    {
        if (transform.childCount == 0)
        {
            Utilities.ComponentError(this, "No child GameObjects.");
            return;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            var component = transform.GetChild(i).GetComponent<Collider2D>();
            if (component is null)
            {
                continue;
            }
            playerCollider = component;
            break;
        }
        if (playerCollider is null)
        {
            Utilities.ComponentError(this, "No Collider2D found.");
            return;
        }
    }

    public void Move(int direction)
    {
        if (direction < 0 || direction >= 8) { return; }

        // TODO: Implement movement.
    }
}
