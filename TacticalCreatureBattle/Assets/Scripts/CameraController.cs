using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed;
    public float MaxSize;

    Camera _mainCamera;

    void OnEnable()
    {
        _mainCamera = GetComponent<Camera>();
        if (_mainCamera == null)
        {
            this.Error("Missing or unavailable Camera.");
            return;
        }
        KeyboardInput.KeyDown += OnKeyDown;
    }

    void OnDisable()
    {
        KeyboardInput.KeyDown -= OnKeyDown;
    }

    void Update()
    {
        transform.position += Time.unscaledDeltaTime * _mainCamera.orthographicSize * Speed / 50
            * (Vector3)KeyboardInput.SmoothedDirection;
    }

    void OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case KeyCode.Minus:
                if (_mainCamera.orthographicSize <= MaxSize - 2)
                {
                    _mainCamera.orthographicSize += 2;
                }
                break;
            case KeyCode.Equals:
                if (_mainCamera.orthographicSize > 2)
                {
                    _mainCamera.orthographicSize -= 2;
                }
                break;
        }
    }
}
