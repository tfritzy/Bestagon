using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class GameTests
{
    [TestMethod]
    public void Game_Constructor()
    {
        Game game = new Game();
        Assert.IsNotNull(game.Board);
        Assert.IsNotNull(game.Players);
        Assert.IsNotNull(game.Board.Projectiles);
    }

    [TestMethod]
    public void Game_BoardSetup()
    {
        Game game = new Game();
        HashSet<Vector2> positions = new HashSet<Vector2>();
        HashSet<int> ids = new HashSet<int>();

        foreach (Hexagon hexagon in game.Board.Hexagons.Values)
        {
            Assert.IsFalse(positions.Contains(hexagon.Position));
            positions.Add(hexagon.Position);

            Assert.IsFalse(ids.Contains(hexagon.Id));
            Assert.IsFalse(hexagon.IsDestroyed);
            ids.Add(hexagon.Id);
        }

        Assert.AreEqual(Constants.RowsPerPlayer * Constants.HexagonsPerRow * 2, game.Board.Hexagons.Count);
    }

    [TestMethod]
    public void Game_InitialBoardStateMessage()
    {
        Game game = new Game();
        Client client1 = TestObjects.BuildClient();
        Client client2 = TestObjects.BuildClient();
        game.AddClient(client1);
        game.AddClient(client2);
        game.Update(game.LastUpdateTime);

        foreach (Client player in game.Players)
        {
            Schema.BoardState boardState = player.MessageLog.First.Value.Unpack<Schema.BoardState>();
            HashSet<Hexagon> uniqueHexagons = new HashSet<Hexagon>();
            foreach (Schema.HexagonSet hexSet in boardState.HexagonSets)
            {
                foreach (Schema.Hexagon hexagon in hexSet.Hexagons)
                {
                    Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Id, hexagon.Id);
                    Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Position.X, hexagon.Position.X);
                    Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Position.Y, hexagon.Position.Y);
                    Assert.AreEqual(game.Board.Hexagons[hexagon.Id].PlayerId, hexSet.PlayerId);
                    Assert.AreEqual(game.Board.Hexagons[hexagon.Id].IsDestroyed, hexagon.IsDestroyed);
                }
            }
        }

        Schema.BoardState emptyBoardState = game.Board.GetBoardState();
        Assert.AreEqual(2, emptyBoardState.HexagonSets.Count);
        Assert.AreEqual(0, emptyBoardState.HexagonSets[0].Hexagons.Count);
        Assert.AreEqual(0, emptyBoardState.HexagonSets[1].Hexagons.Count);
    }

    [TestMethod]
    public void Game_DestroyHexagons()
    {
        Game game = new Game();
        Client client1 = TestObjects.BuildClient();
        Client client2 = TestObjects.BuildClient();
        game.Players.Add(client1);
        game.Players.Add(client2);
        Schema.BoardState originalBoardState = game.Board.GetBoardState();
        game.Board.DestroyHexagon(0);
        Assert.IsTrue(game.Board.Hexagons[0].IsDestroyed);
        Assert.ThrowsException<System.ArgumentException>(() => { game.Board.DestroyHexagon(420); });
        game.Update(DateTime.Now);

        foreach (Client client in game.Players)
        {
            Assert.IsTrue(client.MessageLog.Last.Value.Is(Schema.BoardState.Descriptor));
            Schema.BoardState boardState = client.MessageLog.Last.Value.Unpack<Schema.BoardState>();
            Assert.AreEqual(1, boardState.HexagonSets[0].Hexagons.Count);
            Assert.AreEqual(2, boardState.HexagonSets.Count);
            Assert.AreEqual(0, boardState.HexagonSets[0].Hexagons[0].Id);
            Assert.IsTrue(boardState.HexagonSets[0].Hexagons[0].IsDestroyed);
            Assert.AreEqual(0, boardState.HexagonSets[1].Hexagons.Count);
        }

        game.Board.DestroyHexagon(0);
        Schema.BoardState emptyBoardState = game.Board.GetBoardState();
        Assert.AreEqual(0, emptyBoardState.HexagonSets[0].Hexagons.Count);
    }

    [TestMethod]
    public void Game_CreateProjectile()
    {
        Game game = new Game();
        Client client = TestObjects.BuildClient();
        game.Players.Add(client);
        Vector2 position = new Vector2(1, 2);
        Vector2 velocity = new Vector2(-1, -1);

        game.Board.CreateProjectile(0, position, velocity, (int)ProjectileType.BouncingBall);
        game.Update(game.LastUpdateTime);
        Assert.IsTrue(game.Players[0].MessageLog.First.Value.Is(Schema.ProjectileCreated.Descriptor));
        Schema.ProjectileCreated createdMessage = game.Players[0].MessageLog.First.Value.Unpack<Schema.ProjectileCreated>();
        Projectile projectile = game.Board.Projectiles[0];

        Assert.AreEqual(position, projectile.Position);
        Assert.AreEqual(velocity, projectile.Velocity);

        Assert.AreEqual(1, game.Board.Projectiles.Count);
        CompareVectors(projectile.Position, createdMessage.Position);
        CompareVectors(projectile.Velocity, createdMessage.Velocity);
        Assert.AreEqual(projectile.Mass, createdMessage.Mass);
        Assert.AreEqual(projectile.Radius, createdMessage.Radius);
        Assert.AreEqual(0, createdMessage.Id);
        game.Board.CreateProjectile(0, new Vector2(1, 1), new Vector2(4, 5), (int)ProjectileType.BouncingBall);
        Assert.AreEqual(1, game.Board.Projectiles.Last().Id);

        game.Update(game.LastUpdateTime.AddSeconds(1));
        Assert.AreEqual(position + velocity, projectile.Position);

        game.Update(game.LastUpdateTime.AddSeconds(1));
        Assert.AreEqual(position + velocity * 2, projectile.Position);
    }

    [TestMethod]
    public void Game_PlayerRequestsProjectile()
    {
        Game game = TestObjects.BuildFullRunningGame();
        Schema.ProjectileCreated projCreated = TestObjects.BuildPlayerCreatesProjectile(new Vector2(5, 6), new Vector2(1, 1), ProjectileType.BouncingBall);
        game.Players[0].ClientSendToServer(Any.Pack(projCreated));
        game.Update(game.LastUpdateTime);
        Schema.ProjectileCreated sentProjCreatedMessage = game.Players[0].MessageLog.First.Next.Value.Unpack<Schema.ProjectileCreated>();
        BouncingBall dummyBouncingBall = new BouncingBall(-1, new Vector2(), new Vector2());

        Assert.AreEqual(projCreated.Position, sentProjCreatedMessage.Position);
        Assert.AreEqual(projCreated.Velocity, sentProjCreatedMessage.Velocity);
        Assert.AreEqual(projCreated.Type, sentProjCreatedMessage.Type);
        Assert.AreEqual(dummyBouncingBall.Mass, sentProjCreatedMessage.Mass);
        Assert.AreEqual(dummyBouncingBall.Radius, sentProjCreatedMessage.Radius);
    }

    private void CompareVectors(Vector2 vector, Schema.Vector2 contractVector)
    {
        Assert.AreEqual(vector.X, contractVector.X);
        Assert.AreEqual(vector.Y, contractVector.Y);
    }
}