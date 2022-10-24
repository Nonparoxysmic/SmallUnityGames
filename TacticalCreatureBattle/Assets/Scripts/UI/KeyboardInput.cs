using System;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public static event EventHandler AnyKeyDown;
    public static event EventHandler<DirectionEventArgs> DirectionalInput;

    public static Vector2 SmoothedDirection { get; private set; }

    [SerializeField] float repeatStartDelay;
    [SerializeField] float repeatTime;

    bool _holdingHorz;
    bool _holdingVert;
    float _nextHorzRepeatTime;
    float _nextVertRepeatTime;
    int _lastFrameHorz;
    int _lastFrameVert;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            AnyKeyDown?.Invoke(this, EventArgs.Empty);
        }

        float smoothedHorz = Input.GetAxis("Horizontal");
        float smoothedVert = Input.GetAxis("Vertical");
        SmoothedDirection = new Vector2(smoothedHorz, smoothedVert);

        int rawHorz = (int)Input.GetAxisRaw("Horizontal");
        int rawVert = (int)Input.GetAxisRaw("Vertical");
        float currentFrameTime = Time.unscaledTime;
        int x = 0, y = 0;
        if (rawHorz != _lastFrameHorz)
        {
            _holdingHorz = false;
            _nextHorzRepeatTime = 0;
        }
        if (rawHorz == 0)
        {
            _holdingHorz = false;
            _nextHorzRepeatTime = 0;
        }
        else if (currentFrameTime > _nextHorzRepeatTime)
        {
            x = rawHorz;
            _nextHorzRepeatTime = currentFrameTime + (_holdingHorz ? repeatTime : repeatStartDelay);
            _holdingHorz = true;
        }
        if (rawVert != _lastFrameVert)
        {
            _holdingVert = false;
            _nextVertRepeatTime = 0;
        }
        if (rawVert == 0)
        {
            _holdingVert = false;
            _nextVertRepeatTime = 0;
        }
        else if (currentFrameTime > _nextVertRepeatTime)
        {
            y = rawVert;
            _nextVertRepeatTime = currentFrameTime + (_holdingVert ? repeatTime : repeatStartDelay);
            _holdingVert = true;
        }
        if (x != 0 || y != 0)
        {
            DirectionalInput?.Invoke(this, new DirectionEventArgs(x, y));
        }
        _lastFrameHorz = rawHorz;
        _lastFrameVert = rawVert;
    }
}
