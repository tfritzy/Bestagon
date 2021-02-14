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
        Game game = new Game();
        Assert.IsNotNull(game.Board);
        Assert.IsNotNull(game.Players);
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
            ids.Add(hexagon.Id);
        }


        Assert.AreEqual(Constants.RowsPerPlayer * Constants.HexagonsPerRow * 2, game.Board.Hexagons.Count);
    }

    [TestMethod]
    public void Game_InitialBoardStateMessage()
    {
        Game game = new Game();

        Schema.BoardState boardState = game.Board.GetBoardState();

        HashSet<Hexagon> uniqueHexagons = new HashSet<Hexagon>();
        foreach (Schema.HexagonSet hexSet in boardState.HexagonSets)
        {
            foreach (Schema.Hexagon hexagon in hexSet.Hexagons)
            {
                Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Id, hexagon.Id);
                Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Position.X, hexagon.Position.X);
                Assert.AreEqual(game.Board.Hexagons[hexagon.Id].Position.Y, hexagon.Position.Y);
                Assert.AreEqual(game.Board.Hexagons[hexagon.Id].PlayerId, hexSet.PlayerId);
            }
        }

        boardState = game.Board.GetBoardState();
        Assert.AreEqual(2, boardState.HexagonSets.Count);
        Assert.AreEqual(0, boardState.HexagonSets[0].Hexagons.Count);
        Assert.AreEqual(0, boardState.HexagonSets[1].Hexagons.Count);
    }

    [TestMethod]
    public void Game_DestroyHexagons()
    {
        Game game = new Game();
        Schema.BoardState originalBoardState = game.Board.GetBoardState();
        game.Board.DestroyHexagon(0);
        Assert.IsTrue(game.Board.Hexagons[0].IsDestroyed);
        Assert.ThrowsException<System.ArgumentException>(() => { game.Board.DestroyHexagon(420); });

        Schema.BoardState boardState = game.Board.GetBoardState();
        Assert.AreEqual(2, boardState.HexagonSets.Count);
        Assert.AreEqual(1, boardState.HexagonSets[0].Hexagons.Count);
        Assert.AreEqual(0, boardState.HexagonSets[0].Hexagons[0].Id);
        Assert.AreEqual(0, boardState.HexagonSets[1].Hexagons.Count);

        game.Board.DestroyHexagon(0);
        boardState = game.Board.GetBoardState();
        Assert.AreEqual(0, boardState.HexagonSets[0].Hexagons.Count);
    }
}