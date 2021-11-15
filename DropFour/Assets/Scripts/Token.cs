using System;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] Sprite redToken;
    [SerializeField] Sprite yellowToken;

    SpriteRenderer sr;

    float _acceleration = 100;
    float _maxSpeed = 20;

    bool isFalling;
    float speedY;
    float targetY;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isFalling) return;
        float currentY = gameObject.transform.position.y;
        if (currentY > targetY)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentY + speedY * Time.deltaTime, gameObject.transform.position.z);

            speedY -= Math.Abs(_acceleration) * Time.deltaTime;
            speedY = Math.Max(speedY, -Math.Abs(_maxSpeed));
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

    public void SetPhysics(float acceleration, float maxSpeed)
    {
        _acceleration = acceleration;
        _maxSpeed = maxSpeed;
    }

    public void SetToken(int player)
    {
        if (player == 0)
        {
            sr.sprite = redToken;
        }
        else sr.sprite = yellowToken;
    }
}
