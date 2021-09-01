using System;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] float accelerationY;
    [SerializeField] float maxSpeed;

    bool isFalling;
    float speedY;
    float targetY;

    void Update()
    {
        if (!isFalling) return;
        float currentY = gameObject.transform.position.y;
        if (currentY > targetY)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentY + speedY * Time.deltaTime, gameObject.transform.position.z);

            speedY -= Math.Abs(accelerationY) * Time.deltaTime;
            speedY = Math.Max(speedY, -Math.Abs(maxSpeed));
        }
        if (gameObject.transform.position.y <= targetY)
        {
            isFalling = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, targetY, gameObject.transform.position.z);
            Destroy(this);
        }
    }

    public void SetFallPositionY(float y)
    {
        targetY = y;
        isFalling = true;
    }
}
