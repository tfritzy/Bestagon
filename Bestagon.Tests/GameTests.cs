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
        Dictionary<int, int> playerHexCounts = new Dictionary<int, int>();
        foreach (Hexagon hexagon in game.Board.Hexagons)
        {
            Assert.IsFalse(positions.Contains(hexagon.Position));
            positions.Add(hexagon.Position);

            Assert.IsFalse(ids.Contains(hexagon.Id));
            ids.Add(hexagon.Id);

            if (playerHexCounts.ContainsKey(hexagon.Player) == false)
            {
                playerHexCounts[hexagon.Player] = 0;
            }
            playerHexCounts[hexagon.Player] += 1;
        }

        Assert.IsTrue(playerHexCounts[0] > 0);
        Assert.AreEqual(2, playerHexCounts.Count);
        Assert.AreEqual(playerHexCounts[0], playerHexCounts[1]);
    }

    [TestMethod]
    public void Game_InitialBoardStateMessage()
    {
        Game game = new Game(2);
        // BoardState boardState = game.Board.GetBoardState();
    }
}