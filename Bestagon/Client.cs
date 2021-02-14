using System.Collections.Generic;
using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;

public class Client
{
    public TCPConnection TCPConnection;
    public string Id;
    public Server Server;
    public Game Game;
    public LinkedList<Any> MessageLog;

    public Client(string clientId, Server server)
    {
        this.Id = clientId;
        this.TCPConnection = new TCPConnection(HandleData);
        this.Server = server;
        this.MessageLog = new LinkedList<Any>();
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

        this.MessageLog.AddLast(any);

        if (any.Is(Schema.LookingForGame.Descriptor))
        {
            AskForGame(any.Unpack<Schema.LookingForGame>());
        }
    }

    private void AskForGame(Schema.LookingForGame playerLookingForGame)
    {
        this.Game = Server.FindGame(this);

        Schema.JoinedGame joinedGame = new Schema.JoinedGame
        {
            Username = this.Id,
        };

        this.SendMessage(Any.Pack(joinedGame));
        this.SendMessage(Any.Pack(Game.Board.GetBoardState()));
    }

    private void SendMessage(Any message)
    {
        this.TCPConnection.SendMessage(message);
        this.MessageLog.AddLast(message);
    }
}