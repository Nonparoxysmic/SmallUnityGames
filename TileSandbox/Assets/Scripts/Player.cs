using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float normalSpeed;

    public bool isStrafing;
    public bool onRaft;
    public bool paused;
    public int facingDirection = 2;
    public int[] inventory = new int[4];

    Collider2D playerCollider;
    SpriteRenderer raftSpriteRenderer;

    void Start()
    {
        if (transform.childCount == 0)
        {
            this.Error("No child GameObjects for collider.");
            return;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Collider2D collider = transform.GetChild(i).GetComponent<Collider2D>();
            SpriteRenderer spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (!(collider == null))
            {
                playerCollider = collider;
            }
            if (!(spriteRenderer == null))
            {
                raftSpriteRenderer = spriteRenderer;
            }
        }
        if (playerCollider == null)
        {
            this.Error("No Collider2D found.");
            return;
        }
        if (raftSpriteRenderer == null)
        {
            this.Error("No SpriteRenderer found.");
            return;
        }
    }

    void Update()
    {
        if (transform.position != playerCollider.transform.position)
        {
            transform.position = playerCollider.transform.position;
            playerCollider.transform.localPosition = Vector3.zero;
        }
        raftSpriteRenderer.enabled = onRaft;
    }

    public void Move(int direction)
    {
        if (!direction.InRange(0, 8)) { return; }
        if (paused) { return; }
        float speed = isStrafing ? normalSpeed / 2 : normalSpeed;
        speed = onRaft ? speed / 2 : speed;
        Vector3 move = speed * Time.fixedDeltaTime * Utilities.DirectionVector(direction);
        playerCollider.transform.position += move;
    }

    public void Teleport(int deltaX, int deltaY)
    {
        Vector3 move = new Vector3(deltaX, deltaY, 0);
        playerCollider.transform.position += move;
    }

    public Vector3 ColliderPosition()
    {
        return playerCollider.transform.position;
    }

    public void Collision(bool doCollision)
    {
        playerCollider.enabled = doCollision;
    }
}
