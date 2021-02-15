public class BouncingBall : Projectile
{
    public override ProjectileType Type => ProjectileType.BouncingBall;
    public override float Radius => 0.3f;
    public override float Mass => 1f;

    public BouncingBall(int id, Vector2 position, Vector2 velocity) : base(id, position, velocity)
    {

    }
}