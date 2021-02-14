public class Hexagon
{
    public Vector2 Position;
    public int Id;

    public Hexagon(Vector2 position, int id)
    {
        this.Position = position;
        this.Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is Hexagon == false)
        {
            return false;
        }

        Hexagon otherHex = (Hexagon)obj;

        return (otherHex.Position.Equals(Position) && otherHex.Id == this.Id);
    }

    public Schema.Hexagon ToContract()
    {
        return new Schema.Hexagon
        {
            Id = this.Id,
            Position = new Schema.Vector2
            {
                X = this.Position.X,
                Y = this.Position.Y
            }
        };
    }
}