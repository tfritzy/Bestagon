public class Hexagon
{
    public Vector2 Position;
    public int Id;
    public int Player;

    public Hexagon(Vector2 position, int id, int player)
    {
        this.Position = position;
        this.Id = id;
        this.Player = player;
    }

    public override bool Equals(object obj)
    {
        if (obj is Hexagon == false)
        {
            return false;
        }

        Hexagon otherHex = (Hexagon)obj;

        return (otherHex.Position.Equals(Position) && otherHex.Id == this.Id && otherHex.Player == this.Player);
    }

    public Schema.Hexagon ToContract()
    {
        return new Schema.Hexagon
        {
            Id = this.Id,
            Player = this.Player,
            Position = new Schema.Vector2
            {
                X = this.Position.X,
                Y = this.Position.Y
            }
        };
    }
}