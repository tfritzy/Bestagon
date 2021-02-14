using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class HexagonTests
{
    [TestMethod]
    public void Hexagon_TestEquals()
    {
        Hexagon hex1 = new Hexagon(new Vector2(1, 1.5f), 0, 0);
        Assert.AreEqual(hex1, hex1);
        Hexagon hex2 = new Hexagon(new Vector2(1, 1.5f), 0, 0);
        Assert.AreEqual(hex1, hex2);
        hex2.Position.X = 1.1f;
        Assert.AreNotEqual(hex1, hex2);
        hex2.Position.X = 1;
        hex2.Position.Y = 1.6f;
        Assert.AreNotEqual(hex1, hex2);
        hex2.Position.Y = 1.5f;
        Assert.AreEqual(hex1, hex2);
        hex2.Id = 10;
        Assert.AreNotEqual(hex1, hex2);
        hex2.Id = 0;
        Assert.AreEqual(hex1, hex2);
        hex2 = null;
        Assert.AreNotEqual(hex1, hex2);
        hex1 = null;
        Assert.AreEqual(hex1, hex2);
    }
}