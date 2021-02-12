public class Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object other)
    {
        if (other is Vector2 == false)
        {
            return false;
        }

        return ((Vector2)other).X == this.X &&
               ((Vector2)other).Y == this.Y;
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