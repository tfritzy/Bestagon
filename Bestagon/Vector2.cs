public class Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public Vector2()
    {
        this.X = 0;
        this.Y = 0;
    }

    public override bool Equals(object obj)
    {
        if (this == null || obj == null)
        {
            return false;
        }

        if (obj is Vector2 == false)
        {
            return false;
        }

        return ((Vector2)obj).X == this.X &&
               ((Vector2)obj).Y == this.Y;
    }

    public override int GetHashCode()
    {
        return (int)(X * 196613 + Y * 393241);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.X * b, a.Y * b);
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.X / b, a.Y / b);
    }

    public override string ToString()
    {
        return $"<{this.X}, {this.Y}>";
    }

    public Schema.Vector2 ToContract()
    {
        return new Schema.Vector2
        {
            X = this.X,
            Y = this.Y,
        };
    }
}