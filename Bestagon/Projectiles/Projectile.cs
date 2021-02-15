public abstract class Projectile
{
    public int Id;
    public Vector2 Position;
    public Vector2 Velocity;
    public abstract ProjectileType Type { get; }
    public abstract float Radius { get; }
    public abstract float Mass { get; }

    public Projectile(int Id, Vector2 position, Vector2 velocity)
    {
        this.Position = position;
        this.Velocity = velocity;
        this.Id = Id;
    }

    public Schema.ProjectileCreated BuildCreatedMessage()
    {
        return new Schema.ProjectileCreated
        {
            Id = this.Id,
            Mass = Mass,
            Position = this.Position.ToContract(),
            Radius = this.Radius,
            Type = (int)this.Type,
            Velocity = this.Velocity.ToContract(),
        };
    }
}