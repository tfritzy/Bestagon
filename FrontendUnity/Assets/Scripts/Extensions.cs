using UnityEngine;

public static class Extensions
{
    public static Schema.Vector2 ToContract(this Vector2 vector)
    {
        return new Schema.Vector2
        {
            X = vector.x,
            Y = vector.y
        };
    }

    public static Vector2 ToInternal(this Schema.Vector2 vector)
    {
        return new Vector2(vector.X, vector.Y);
    }
}
