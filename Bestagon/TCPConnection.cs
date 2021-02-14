using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

public class TCPConnection
{
    public TcpClient TcpClient;
    private byte[] receiveBuffer;
    public NetworkStream Stream;
    public Packet ReceivedData;
    public LinkedList<Any> MessageQueue;


    public TCPConnection()
    {
        TcpClient = new TcpClient
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

    public void SendMessage(Any any)
    {
        byte[] bytes = any.ToByteArray();
        Console.WriteLine($"Sending {bytes.Length} byte message");

        if (Stream != null)
        {
            Stream.Write(bytes, 0, bytes.Length);
        }
    }

    public void Connect(TcpClient client)
    {
        TcpClient = client;
        TcpClient.ReceiveBufferSize = Constants.DEFAULT_BUFFER_SIZE;
        TcpClient.SendBufferSize = Constants.DEFAULT_BUFFER_SIZE;

        Stream = TcpClient.GetStream();

        Stream.BeginRead(receiveBuffer, 0, Constants.DEFAULT_BUFFER_SIZE, ReceiveCallback, Stream);
    }

    public void Connect(string ip, int port)
    {
        TcpClient.BeginConnect(ip, port, ConnectCallback, TcpClient);
    }

    private void ConnectCallback(IAsyncResult _result)
    {
        TcpClient.EndConnect(_result);

        if (!TcpClient.Connected)
        {
            return;
        }

        Stream = TcpClient.GetStream();

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