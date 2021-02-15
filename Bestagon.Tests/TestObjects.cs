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
            Username = client.Username,
        };
    }

    public static Game BuildFullRunningGame()
    {
        Game game = new Game();
        Client client1 = TestObjects.BuildClient();
        Client client2 = TestObjects.BuildClient();
        game.AddClient(client1);
        game.AddClient(client2);
        return game;
    }

    public static Schema.ProjectileCreated BuildPlayerCreatesProjectile(Vector2 position, Vector2 velocity, ProjectileType type)
    {
        return new Schema.ProjectileCreated
        {
            Position = position.ToContract(),
            Type = (int)type,
            Velocity = velocity.ToContract()
        };
    }
}