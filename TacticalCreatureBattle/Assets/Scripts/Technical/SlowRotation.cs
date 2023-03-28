using UnityEngine;

public class SlowRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 120 * Time.deltaTime);
    }
}
