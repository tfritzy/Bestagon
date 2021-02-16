using System;

public static class Extensions
{
    public static Vector2 ToInternal(this Schema.Vector2 vector2)
    {
        return new Vector2(vector2.X, vector2.Y);
    }
}