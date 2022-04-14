public static class Extensions
{
    public static bool EqualsOneOf(this int n, int a, int b, int c, int d)
    {
        return n == a || n == b || n == c || n == d;
    }
}
