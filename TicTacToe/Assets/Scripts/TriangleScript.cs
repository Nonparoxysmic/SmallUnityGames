using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class MyEventTest : UnityEvent<GameObject> { }

public class TriangleScript : MonoBehaviour
{
    public MyEventTest triangleEvent;

    private void OnMouseDown()
    {
        triangleEvent.Invoke(gameObject);
    }
}
