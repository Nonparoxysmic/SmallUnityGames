using System;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    float targetY;

    void Awake()
    {
        targetY = gameObject.transform.position.y;
    }

    void Update()
    {
        float currentY = gameObject.transform.position.y;
        if (currentY != targetY)
        {
            float diff = targetY - currentY;
            int direction = Math.Sign(diff);
            if (Math.Abs(diff) > 0.05f)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, currentY + direction * 0.01f, gameObject.transform.position.z);
            }
            else
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, targetY, gameObject.transform.position.z);
            }
        }
    }

    public void SetFallPositionY(float y)
    {
        targetY = y;
    }
}
