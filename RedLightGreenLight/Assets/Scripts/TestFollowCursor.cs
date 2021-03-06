using System;
using UnityEngine;

public class TestFollowCursor : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z += 1;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
