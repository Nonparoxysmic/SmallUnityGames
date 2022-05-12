using UnityEngine;

public class CoreFocus : MonoBehaviour
{
    public GameObject target;
    
    public float speedFactor;

    void FixedUpdate()
    {
        if (target is null) { return; }

        Vector3 delta = target.transform.position - transform.position;
        if (delta.magnitude < 0.001 && speedFactor > 0)
        {
            transform.position = target.transform.position;
            return;
        }
        if (speedFactor < 0 || speedFactor > 1)
        {
            Utilities.ComponentError(this, nameof(speedFactor) + " must be in the range [0, 1].");
            return;
        }
        transform.position += speedFactor * delta;
    }
}
