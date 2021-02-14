
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

public class Server
{
    private static TcpListener tcpListener;
    private static List<Client> clients = new List<Client>();
    private List<Thread> threads = new List<Thread>();

    public Server(int port, int maxPlayers)
    {
        this.MaxPlayersPerGame = maxPlayers;
        this.Port = port;
        this.OpenGames = new LinkedList<Game>();
        this.RunningGames = new LinkedList<Game>();
    }

    public int Port { get; }
    public int MaxPlayersPerGame { get; }
    public int PlayerCount { get { return clients.Count; } }
    public LinkedList<Game> OpenGames;
    public LinkedList<Game> RunningGames;

    public void Start()
    {
        Console.WriteLine("Starting server...");

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        Console.WriteLine($"Server started on port {Port}.");
    }

    private void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient tcpClient = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Console.WriteLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

        Client client = new Client(Guid.NewGuid().ToString("N"), this);
        client.Connect(tcpClient);
        AddConnection(client);
    }

    public bool AddConnection(Client client)
    {
        if (PlayerCount >= MaxPlayersPerGame)
        {
            return false;
        }

        clients.Add(client);

        return true;
    }

    public void Update()
    {
        while (true)
        {
            try
            {
                foreach (Client client in clients)
                {
                    client.DrainMessageQueue().Wait();
                }
            }
            catch (InvalidOperationException) { }
        }
    }

    public Game FindGame(Client client)
    {
        if (client.Game != null)
        {
            return client.Game;
        }

        if (OpenGames.Count > 0)
        {
            OpenGames.First.Value.Players.Add(client);
            if (OpenGames.First.Value.IsFull())
            {
                Game game = OpenGames.First.Value;
                OpenGames.RemoveFirst();
                RunningGames.AddLast(game);
                return game;
            }

            return OpenGames.First.Value;
        }
        else
        {
            Game game = new Game(MaxPlayersPerGame);
            OpenGames.AddLast(game);
            game.Players.Add(client);
            return OpenGames.Last.Value;
        }
    }
}
