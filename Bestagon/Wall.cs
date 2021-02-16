public class Wall
{
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 Direction { get; }

    public Wall(Vector2 start, Vector2 end)
    {
        this.startPos = start;
        this.endPos = end;
        this.Direction = endPos - startPos;
    }
}