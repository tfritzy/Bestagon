using System;
using System.IO;
using System.Net.Sockets;
using Moq;

public static class TestObjects
{
    public static Client BuildClient(Server server = null)
    {
        Client client = new Client(Guid.NewGuid().ToString("N"), server ?? new Server(8080));
        // var moqTcpClient = new Mock<TcpClient>();
        // var moqSocket = new Mock<Socket>(SocketType.Stream, ProtocolType.Tcp);
        // var moqStream = new Mock<NetworkStream>();
        // client.TCPConnection.TcpClient = moqTcpClient.Object;
        // client.TCPConnection.TcpClient.Client = moqSocket.Object;
        return client;
    }

    public static Schema.LookingForGame BuildLookingForGame(this Client client)
    {
        return new Schema.LookingForGame()
        {
            Username = client.Id,
        };
    }
}