using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ServerTests
{
    [TestMethod]
    public void Server_TestPortAndPlayers()
    {
        Server server = new Server(8080, 4);
        Assert.AreEqual(4, server.MaxPlayersPerGame);
        Assert.AreEqual(8080, server.Port);
    }

    [TestMethod]
    public void Server_AddConnection()
    {
        Server server = new Server(8080, 1);
        Client client = new Client("0", server);
        Client client2 = new Client("1", server);

        Assert.AreEqual(0, server.PlayerCount);

        Assert.IsTrue(server.AddConnection(client));
        Assert.AreEqual(1, server.PlayerCount);

        Assert.IsFalse(server.AddConnection(client2));
        Assert.AreEqual(1, server.PlayerCount);
    }

    [TestMethod]
    public void Server_ClientLookingForGame()
    {
        Server server = new Server(8080, 1);
        Client client = TestObjects.BuildClient(server);
        client.ClientSendToServer(Any.Pack(client.BuildLookingForGame()));
        Assert.IsTrue(client.MessageLog.First.Value.Is(Schema.LookingForGame.Descriptor));
        Assert.IsTrue(client.MessageLog.Last.Value.Is(Schema.JoinedGame.Descriptor));
    }
}
