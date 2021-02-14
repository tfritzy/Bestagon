using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class GameTests
{
    [TestMethod]
    public void Game_Constructor()
    {
        Game game = new Game(2);
        Assert.IsNotNull(game.Board);
        Assert.AreEqual(2, game.MaxPlayers);
        Assert.IsNotNull(game.Players);
    }

    [TestMethod]
    public void Game_BoardSetup()
    {
        Game game = new Game(2);

        HashSet<Vector2> positions = new HashSet<Vector2>();
        HashSet<int> ids = new HashSet<int>();
        foreach (int playerId in game.Board.Hexagons.Keys)
        {
            foreach (Hexagon hexagon in game.Board.Hexagons[playerId].Values)
            {
                Assert.IsFalse(positions.Contains(hexagon.Position));
                positions.Add(hexagon.Position);

                Assert.IsFalse(ids.Contains(hexagon.Id));
                ids.Add(hexagon.Id);
            }
        }

        Assert.AreEqual(2, game.Board.Hexagons.Count);
        Assert.IsTrue(game.Board.Hexagons[0].Count > 0);
        Assert.AreEqual(game.Board.Hexagons[0].Count, game.Board.Hexagons[1].Count);
    }

    [TestMethod]
    public void Game_InitialBoardStateMessage()
    {
        Game game = new Game(2);
        Schema.BoardState boardState = game.Board.GetBoardState();

        HashSet<Hexagon> uniqueHexagons = new HashSet<Hexagon>();
        foreach (Schema.HexagonSet hexSet in boardState.PlayerHexagons)
        {
            foreach (Schema.Hexagon hexagon in hexSet.Hexagons)
            {
                Assert.AreEqual(game.Board.Hexagons[hexSet.PlayerId][hexagon.Id].Id, hexagon.Id);
                Assert.AreEqual(game.Board.Hexagons[hexSet.PlayerId][hexagon.Id].Position.X, hexagon.Position.X);
                Assert.AreEqual(game.Board.Hexagons[hexSet.PlayerId][hexagon.Id].Position.Y, hexagon.Position.Y);
            }
        }
    }
}