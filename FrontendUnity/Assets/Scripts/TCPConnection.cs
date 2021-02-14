using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

public class TCPConnection
{
    public TcpClient socket;
    private byte[] receiveBuffer;
    public NetworkStream Stream;
    public Packet ReceivedData;
    public LinkedList<Any> MessageQueue;


    public TCPConnection()
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = Constants.DEFAULT_BUFFER_SIZE,
            SendBufferSize = Constants.DEFAULT_BUFFER_SIZE,
        };

        receiveBuffer = new byte[Constants.DEFAULT_BUFFER_SIZE];
        ReceivedData = new Packet();
        MessageQueue = new LinkedList<Any>();
    }

    public byte[] ReceiveBuffer
    {
        get { return receiveBuffer; }
        set
        {
            Array.Copy(value, receiveBuffer, value.Length);
        }
    }

    public async Task SendMessage(Any any)
    {
        byte[] bytes = any.ToByteArray();
        Console.WriteLine($"Sending {bytes.Length} byte message");
        Stream.Write(bytes, 0, bytes.Length);
    }

    public void Connect(TcpClient client)
    {
        socket = client;
        socket.ReceiveBufferSize = Constants.DEFAULT_BUFFER_SIZE;
        socket.SendBufferSize = Constants.DEFAULT_BUFFER_SIZE;

        Stream = socket.GetStream();
        Stream.WriteTimeout = 1000;

        Stream.BeginRead(receiveBuffer, 0, Constants.DEFAULT_BUFFER_SIZE, ReceiveCallback, Stream);
    }

    public void Connect(string ip, int port)
    {
        socket.BeginConnect(ip, port, ConnectCallback, socket);
    }

    private void ConnectCallback(IAsyncResult _result)
    {
        socket.EndConnect(_result);

        if (!socket.Connected)
        {
            return;
        }

        Stream = socket.GetStream();

        Stream.BeginRead(ReceiveBuffer, 0, Constants.DEFAULT_BUFFER_SIZE, ReceiveCallback, Stream);
    }

    private void ReceiveCallback(IAsyncResult _result)
    {
        int packetLength = Stream.EndRead(_result);
        Console.WriteLine($"Received {packetLength} byte message");
        Stream.BeginRead(receiveBuffer, 0, Constants.DEFAULT_BUFFER_SIZE, ReceiveCallback, Stream);
        ExtractDataFromBuffer(packetLength);
    }

    public void ExtractDataFromBuffer(int dataEndIndex)
    {
        this.ReceivedData.SetContents(ReceiveBuffer, dataEndIndex);
        MessageQueue.AddLast(this.ReceivedData.Message);
    }
}