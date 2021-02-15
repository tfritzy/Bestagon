using System;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ClientTests
{
    [TestMethod]
    public void Client_Constructor()
    {
        Server server = new Server(8080);
        string id = Guid.NewGuid().ToString("N");
        Client client = new Client(id, server);
        Assert.AreEqual(id, client.Username);
        Assert.AreEqual(server, client.Server);
    }

    [TestMethod]
    public void Client_TCPConnection()
    {
        Client client = TestObjects.BuildClient();
        Assert.IsNotNull(client.TCPConnection);
        Assert.IsNotNull(client.TCPConnection.ReceiveBuffer);
        Assert.AreEqual(Constants.DEFAULT_BUFFER_SIZE, client.TCPConnection.ReceiveBuffer.Length);
        Assert.IsNotNull(client.TCPConnection.ReceivedData);
    }

    [TestMethod]
    public void Client_ExtractDataFromConnection()
    {
        Client client = TestObjects.BuildClient();
        Schema.LookingForGame player = new Schema.LookingForGame() { Username = "Tyler" };
        byte[] bytes = Any.Pack(player).ToByteArray();
        client.TCPConnection.ReceiveBuffer = bytes;
        Assert.AreEqual(Constants.DEFAULT_BUFFER_SIZE, client.TCPConnection.ReceiveBuffer.Length);

        client.TCPConnection.ExtractDataFromBuffer(bytes.Length);

        Schema.LookingForGame readJoinedGame = client.TCPConnection.ReceivedData.Message.Unpack<Schema.LookingForGame>();
        Assert.AreEqual(player, readJoinedGame);
    }

    [TestMethod]
    public void Client_ExtractDataFromConnection_InvalidData()
    {
        Client client = TestObjects.BuildClient();
        byte[] bytes = Encoding.ASCII.GetBytes("Invalid data");
        client.TCPConnection.ReceiveBuffer = bytes;

        client.TCPConnection.ExtractDataFromBuffer(bytes.Length);

        Assert.IsNull(client.TCPConnection.ReceivedData.Message);
    }

    [TestMethod]
    public void Client_HandlePlayerLookingForGame()
    {
        Client client = TestObjects.BuildClient();
        Schema.LookingForGame playerLookingForGame = client.BuildLookingForGame();
        client.ClientSendToServer(Any.Pack(playerLookingForGame));

        Assert.IsNotNull(client.Game);
        CollectionAssert.Contains(client.Game.Players, client);
        Game originalGame = client.Game;

        client.ClientSendToServer(Any.Pack(playerLookingForGame));
        Assert.AreEqual(1, client.Game.Players.Count);
        Assert.AreEqual(originalGame, client.Game);
        Assert.AreEqual(1, client.Server.OpenGames.Count);
    }

    [TestMethod]
    public void Client_HandleMultiplePlayersLookingForGame()
    {
        Server server = new Server(8080);
        Client client1 = TestObjects.BuildClient(server);
        Client client2 = TestObjects.BuildClient(server);
        Client client3 = TestObjects.BuildClient(server);

        client1.ClientSendToServer(Any.Pack(client1.BuildLookingForGame()));
        client2.ClientSendToServer(Any.Pack(client2.BuildLookingForGame()));
        client3.ClientSendToServer(Any.Pack(client3.BuildLookingForGame()));

        Assert.AreEqual(1, server.OpenGames.Count);
        Assert.AreEqual(1, server.RunningGames.Count);
        CollectionAssert.Contains(server.RunningGames.First.Value.Players, client1);
        CollectionAssert.Contains(server.RunningGames.First.Value.Players, client2);
        CollectionAssert.DoesNotContain(server.RunningGames.First.Value.Players, client3);
        CollectionAssert.Contains(server.OpenGames.First.Value.Players, client3);
        Assert.AreEqual(0, client1.PlayerId);
        Assert.AreEqual(1, client2.PlayerId);
        Assert.AreEqual(0, client3.PlayerId);
    }
}
