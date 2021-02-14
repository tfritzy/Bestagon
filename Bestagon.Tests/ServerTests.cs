using Google.Protobuf;
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
        Server server = new Server(8080, 2);
        Client client = TestObjects.BuildClient(server);
        client.ClientSendToServer(Any.Pack(client.BuildLookingForGame()));

        Assert.IsTrue(client.MessageLog.First.Value.Is(Schema.LookingForGame.Descriptor));

        Schema.JoinedGame joinedGameResponse = client.MessageLog.First.Next.Value.Unpack<Schema.JoinedGame>();
        Assert.AreEqual(client.Id, joinedGameResponse.Username);

        Assert.IsTrue(client.MessageLog.Last.Value.Is(Schema.BoardState.Descriptor));
        Schema.BoardState boardState = client.MessageLog.Last.Value.Unpack<Schema.BoardState>();
        byte[] bytes = boardState.ToByteArray();
        int totalHexagonCount = boardState.PlayerHexagons[0].Hexagons.Count + boardState.PlayerHexagons[1].Hexagons.Count;
        Assert.AreEqual(Constants.RowsPerPlayer * Constants.HexagonsPerRow * 2, totalHexagonCount);
    }
}
