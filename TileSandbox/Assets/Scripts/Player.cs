using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float normalSpeed;

    [HideInInspector] public bool isStrafing;
    [HideInInspector] public int facingDirection = 2;

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

    void Update()
    {
        if (transform.position != playerCollider.transform.position)
        {
            transform.position = playerCollider.transform.position;
            playerCollider.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void Move(int direction)
    {
        if (direction < 0 || direction >= 8) { return; }

        float speed = isStrafing ? normalSpeed / 2 : normalSpeed;
        Vector3 move = speed * Time.fixedDeltaTime * Utilities.DirectionVector(direction);
        playerCollider.transform.position += move;
    }

    public Vector3 ColliderPosition()
    {
        return playerCollider.transform.position;
    }
}
