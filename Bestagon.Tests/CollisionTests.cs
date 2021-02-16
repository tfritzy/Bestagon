using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CollisionTests
{
    [TestMethod]
    public void Collision_SimpleLines1()
    {
        Vector2 line1Start = new Vector2(0, 0);
        Vector2 line1End = new Vector2(2, -2);
        Vector2 line2Start = new Vector2(0, -1);
        Vector2 line2End = new Vector2(0.5f, -1);

        Collisions.FindIntersection(
            line1Start, line1End, line2Start, line2End,
            out bool doIntersect,
            out bool doSegmentsIntersect,
            out Vector2 intersection);

        Assert.IsTrue(doIntersect);
        Assert.IsTrue(doSegmentsIntersect);
        Assert.AreEqual(new Vector2(1, -1), intersection);
    }

    [TestMethod]
    public void Collision_OnCorner()
    {
        Projectile projectile = new BouncingBall(0, new Vector2(0, 0), new Vector2(1, 0));
        Wall wall = new Wall(new Vector2(-5, 10), new Vector2(4.9f, .1f));

        Vector2 collisionPoint = projectile.GetCollisionPoint(wall);
        AssertAreClose(new Vector2(4.7878675f, 0.21213183f), collisionPoint);
    }

    [TestMethod]
    public void Collision_CompleteMiss()
    {
        Projectile projectile = new BouncingBall(0, new Vector2(0, 0), new Vector2(1, 0));
        Wall wall = new Wall(new Vector2(0, 10), new Vector2(0, 5));

        Vector2 collisionPoint = projectile.GetCollisionPoint(wall);
        Assert.AreEqual(Vector2.MinValue, collisionPoint);
    }

    [TestMethod]
    public void Collision_HeadOn()
    {
        Projectile projectile = new BouncingBall(0, new Vector2(0, 0), new Vector2(1, 0));
        Wall wall = new Wall(new Vector2(0, 10), new Vector2(0, -10f));

        Vector2 collisionPoint = projectile.GetCollisionPoint(wall);
        AssertAreClose(new Vector2(0, 0), collisionPoint);
    }

    public void AssertAreClose(Vector2 v1, Vector2 v2)
    {
        Assert.IsTrue(MathF.Abs(v1.X - v2.X) < .0001f);
        Assert.IsTrue(MathF.Abs(v1.Y - v2.Y) < .0001f);
    }
}