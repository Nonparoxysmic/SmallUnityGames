using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float normalSpeed;

    Collider2D playerCollider;

    void Start()
    {
        if (transform.childCount == 0)
        {
            // TODO: Handle errors.
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
            // TODO: Handle errors.
        }
    }

    void Move(int direction)
    {
        if (direction < 0 || direction >= 8) { return; }

        // TODO: Implement movement.
    }
}
