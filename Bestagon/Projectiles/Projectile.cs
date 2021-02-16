using System;

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

    public Vector2 GetCollisionPoint(Wall wall)
    {
        Vector2 forwardUnit = this.Position + this.Velocity.Normalized;
        Vector2 farForward = this.Velocity * 10000000f;

        Vector2 c0Start = this.Position + forwardUnit.PerpendicularClockwise * Radius;
        Vector2 c0End = c0Start + farForward;

        Vector2 c1Start = this.Position;
        Vector2 c1End = c1Start + farForward;

        Vector2 c2Start = this.Position + forwardUnit.PerpendicularCounterClockwise * Radius;
        Vector2 c2End = c2Start + farForward;

        Collisions.FindIntersection(
            c0Start, c0End, wall.startPos, wall.endPos,
            out bool c0Intersects,
            out bool c0SegmentsIntersect,
            out Vector2 c0IntersectionPoint);

        Collisions.FindIntersection(
            c1Start, c1End, wall.startPos, wall.endPos,
            out bool c1Intersects,
            out bool c1SegmentsIntersect,
            out Vector2 c1IntersectionPoint);

        float collisionAngle = MathF.Acos(this.Velocity.Dot(wall.Direction) / (this.Velocity.Magnitude * wall.Direction.Magnitude));
        float distanceCollisionPointIsFromC1Intersection = Radius / MathF.Tan(collisionAngle);

        if (c0SegmentsIntersect)
        {
            return c1IntersectionPoint + (c0IntersectionPoint - c1IntersectionPoint).Normalized * distanceCollisionPointIsFromC1Intersection;
        }

        Collisions.FindIntersection(
            c2Start, c2End, wall.startPos, wall.endPos,
            out bool c2Intersects,
            out bool c2SegmentsIntersect,
            out Vector2 c2IntersectionPoint);

        if (c2SegmentsIntersect)
        {
            return c1IntersectionPoint + (c2IntersectionPoint - c1IntersectionPoint).Normalized * distanceCollisionPointIsFromC1Intersection;
        }

        return Vector2.MinValue;
    }
}