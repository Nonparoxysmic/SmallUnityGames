using UnityEngine;

public class Noise
{
    public Noise()
    {

    }

    public float Value(int x, int y)
    {
        if (Mathf.Abs(x) < 2 && Mathf.Abs(y) < 2)
        {
            return Random.Range(0.25f, 0.9999f);
        }
        return Random.Range(0, 0.9999f);
    }
}
