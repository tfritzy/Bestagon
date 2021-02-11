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

    public void Connect(string ip, int port)
    {
        TCPConnection.Connect(ip, port);
    }

    public void HandleData(Any any)
    {
        if (any == null)
        {
            return;
        }

        if (any.Is(PlayerLookingForGame.Descriptor))
        {
            AskForGame(any.Unpack<PlayerLookingForGame>());
        }
    }

    private void AskForGame(PlayerLookingForGame playerLookingForGame)
    {
        this.Game = Server.FindGame(this);
    }
}