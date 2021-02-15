using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

public class Client
{
    public TCPConnection TCPConnection;
    public string Username;
    public Server Server;
    public Game Game;
    public LinkedList<Any> MessageLog;
    public int PlayerId;

    public Client(string clientId, Server server)
    {
        this.Username = clientId;
        this.TCPConnection = new TCPConnection();
        this.Server = server;
        this.MessageLog = new LinkedList<Any>();
    }

    public void Connect(TcpClient client)
    {
        TCPConnection.Connect(client);
    }

    public void DrainMessageQueue()
    {
        while (TCPConnection.MessageQueue.Count > 0)
        {
            HandleData(TCPConnection.MessageQueue.First.Value);
            TCPConnection.MessageQueue.RemoveFirst();
        }
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
        else if (any.Is(Schema.ProjectileCreated.Descriptor))
        {
            // TODO: Remove
            if (Game == null)
            {
                return;
            }
            CreateProjectile(any.Unpack<Schema.ProjectileCreated>());
        }
    }

    private void AskForGame(Schema.LookingForGame playerLookingForGame)
    {
        this.Game = Server.FindGame(this);

        Schema.JoinedGame joinedGame = new Schema.JoinedGame
        {
            Username = this.Username,
        };

        Console.WriteLine("Telling player they got a game");
        this.SendMessage(Any.Pack(joinedGame));

        Console.WriteLine("Telling player initial board state");
        this.SendMessage(Any.Pack(Game.Board.GetBoardState()));
    }

    private void CreateProjectile(Schema.ProjectileCreated projectileCreated)
    {
        this.Game.Board.CreateProjectile(this.PlayerId, projectileCreated.Position.ToInternal(), projectileCreated.Velocity.ToInternal(), projectileCreated.Type);
    }

    public void SendMessage(Any message)
    {
        this.TCPConnection.SendMessage(message);
        this.MessageLog.AddLast(message);
    }
}