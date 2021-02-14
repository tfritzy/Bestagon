public class Hexagon
{
    public Vector2 Position;
    public int Id;
    public bool IsDestroyed;
    public int PlayerId;

    public Hexagon(Vector2 position, int id, int playerId)
    {
        this.Position = position;
        this.Id = id;
        this.IsDestroyed = false;
        this.PlayerId = playerId;
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

    public void Destroy()
    {
        this.IsDestroyed = true;
    }

    public Schema.Hexagon ToContract()
    {
        return new Schema.Hexagon
        {
            Id = this.Id,
            IsDestroyed = this.IsDestroyed,
            Position = new Schema.Vector2
            {
                X = this.Position.X,
                Y = this.Position.Y
            }
        };
    }

    public override int GetHashCode()
    {
        return Id * 786433 + Position.GetHashCode();
    }
}