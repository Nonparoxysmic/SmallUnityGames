using UnityEngine;

public static class Extensions
{
    public static bool EqualsOneOf(this int N, int a, int b, int c, int d)
    {
        return N == a || N == b || N == c || N == d;
    }

    public static void Error(this Component component, string message)
    {
        Debug.LogError(component.name + ": " + component.GetType() + ": " + message);
        if (component is Behaviour behaviour)
        {
            behaviour.enabled = false;
        }
    }
}
