
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
public class Server
{
    private static TcpListener tcpListener;
    private static List<Client> clients = new List<Client>();

    public Server(int port, int maxPlayers)
    {
        this.MaxPlayers = maxPlayers;
        this.Port = port;
    }

    public int Port { get; }
    public int MaxPlayers { get; }
    public int CurrentPlayers { get { return clients.Count; } }

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
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        Client client = new Client(Guid.NewGuid().ToString("N"));
        AddConnection(client);

        Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    public bool AddConnection(Client client)
    {
        if (CurrentPlayers >= MaxPlayers)
        {
            return false;
        }

        clients.Add(client);

        return true;
    }
}
