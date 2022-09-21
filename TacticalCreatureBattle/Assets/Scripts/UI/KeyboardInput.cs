using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public static event EventHandler AnyKeyDown;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            AnyKeyDown?.Invoke(this, EventArgs.Empty);
        }
    }
}
