using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;

public class Client
{
    public TCPConnection TCPConnection;
    public string Id;
    public Server Server;
    public Game Game;

    public Client(string clientId, Server server)
    {
        this.Id = clientId;
        this.TCPConnection = new TCPConnection(HandleData);
        this.Server = server;
    }

    public void Connect(TcpClient client)
    {
        TCPConnection.Connect(client);
    }

    public void HandleData(Any any)
    {
        if (any == null)
        {
            return;
        }

        if (any.Is(Schema.LookingForGame.Descriptor))
        {
            AskForGame(any.Unpack<Schema.LookingForGame>());
        }
    }

    private void AskForGame(Schema.LookingForGame playerLookingForGame)
    {
        this.Game = Server.FindGame(this);
    }
}