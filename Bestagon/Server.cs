
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

    public Server(int port)
    {
        this.Port = port;
        this.OpenGames = new LinkedList<Game>();
        this.RunningGames = new LinkedList<Game>();
        this.lastUpdateTime = DateTime.Now;
    }

    public int Port { get; }
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
        clients.Add(client);

        return true;
    }

    LinkedList<double> timeBetweenUpdates = new LinkedList<double>();
    DateTime lastUpdateTime;
    private double averageTimeBetweenUpdates;
    private int printFPSCounter = 0;
    public void Update()
    {
        while (true)
        {
            UpdateIteration();
            double delta = (DateTime.Now - lastUpdateTime).TotalMilliseconds;
            timeBetweenUpdates.AddLast(delta);
            averageTimeBetweenUpdates += delta;
            lastUpdateTime = DateTime.Now;

            if (printFPSCounter > 10000000)
            {
                Console.WriteLine($"Server running at {1000f / (averageTimeBetweenUpdates / 1000)} FPS");
                printFPSCounter = 0;
            }

            if (timeBetweenUpdates.Count > 1000)
            {
                averageTimeBetweenUpdates -= timeBetweenUpdates.First.Value;
                timeBetweenUpdates.RemoveFirst();
            }


            printFPSCounter += 1;
        }
    }

    public void UpdateIteration(DateTime time = default)
    {
        try
        {
            foreach (Client client in clients)
            {
                client.DrainMessageQueue();
            }
        }
        catch (InvalidOperationException) { }

        foreach (Game g in RunningGames)
        {
            g.Update(time == default ? DateTime.Now : time);
        }

        // TODO: For testing purposes so I don't have to create a second client. Remove later.
        foreach (Game g in OpenGames)
        {
            g.Update(time == default ? DateTime.Now : time);
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
            OpenGames.First.Value.AddClient(client);
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
            Game game = new Game();
            OpenGames.AddLast(game);
            game.AddClient(client);
            return OpenGames.Last.Value;
        }
    }
}
