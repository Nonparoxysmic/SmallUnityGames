using System;
using UnityEngine;

public abstract class BaseEventArgs<T> : EventArgs
{
    protected T _data;
}

public class DirectionEventArgs : BaseEventArgs<Vector2Int>
{
    public Vector2Int Direction { get => _data; }

    public DirectionEventArgs(Vector2Int vector)
    {
        _data = vector;
    }

    public DirectionEventArgs(int x, int y)
    {
        _data = new Vector2Int(x, y);
    }
}

public class IntegerEventArgs : BaseEventArgs<int>
{
    public int Data { get => _data; }

    public IntegerEventArgs(int integer)
    {
        _data = integer;
    }
}

public class KeyEventArgs : BaseEventArgs<KeyCode>
{
    public KeyCode KeyCode { get => _data; }

    public KeyEventArgs(KeyCode keyCode)
    {
        _data = keyCode;
    }
}
