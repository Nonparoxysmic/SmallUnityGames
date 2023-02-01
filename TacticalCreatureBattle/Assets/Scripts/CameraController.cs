using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static bool AllowFreeMovement { get; set; }

    public float Speed;
    public float MaxSize;

    static CameraController _instance;

    Camera _mainCamera;

    void OnEnable()
    {
        _mainCamera = GetComponent<Camera>();
        if (_mainCamera == null)
        {
            this.Error("Missing or unavailable Camera.");
            return;
        }
        _instance = this;
        KeyboardInput.KeyDown += OnKeyDown;
    }

    void OnDisable()
    {
        KeyboardInput.KeyDown -= OnKeyDown;
    }

    void Update()
    {
        if (AllowFreeMovement)
        {
            transform.position += Time.unscaledDeltaTime * _mainCamera.orthographicSize * Speed / 50
                * (Vector3)KeyboardInput.SmoothedDirection;
        }
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

    public static void LookAtUnit(UnitController unit)
    {
        _instance.transform.position = new Vector3
            (
                unit.ViewCenter.x,
                unit.ViewCenter.y,
                _instance.transform.position.z
            );
    }
}
