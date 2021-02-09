using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ServerTests
{
    [TestMethod]
    public void Server_TestPortAndPlayers()
    {
        Server server = new Server(8080, 4);
        Assert.AreEqual(4, server.MaxPlayers);
        Assert.AreEqual(8080, server.Port);
    }

    [TestMethod]
    public void Server_AddConnection()
    {
        Server server = new Server(8080, 1);
        Client client = new Client("0");
        Client client2 = new Client("1");

        Assert.IsTrue(server.AddConnection(client));
        Assert.AreEqual(1, server.CurrentPlayers);

        Assert.IsFalse(server.AddConnection(client2));
        Assert.AreEqual(1, server.CurrentPlayers);
    }
}
