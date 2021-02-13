public class Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
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

    public override string ToString()
    {
        return $"<{this.X}, {this.Y}>";
    }
}