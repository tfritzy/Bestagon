using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;

public class TCPConnection
{
    public TcpClient socket;
    public const int DATA_BUFFER_SIZE = 1024;
    private byte[] receiveBuffer;
    public NetworkStream Stream;
    public Packet ReceivedData;
    public Action<Any> HandleData;

    public TCPConnection(Action<Any> handleData)
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = DATA_BUFFER_SIZE,
            SendBufferSize = DATA_BUFFER_SIZE,
        };

        receiveBuffer = new byte[DATA_BUFFER_SIZE];
        ReceivedData = new Packet();
        HandleData = handleData;
    }

    public byte[] ReceiveBuffer
    {
        get { return receiveBuffer; }
        set
        {
            Array.Copy(value, receiveBuffer, value.Length);
        }
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

        Stream.BeginRead(ReceiveBuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult _result)
    {
        try
        {
            int packetLength = Stream.EndRead(_result);
            ExtractDataFromBuffer(packetLength);
        }
        catch
        {
            // Disconnect();
        }
    }

    public void ExtractDataFromBuffer(int dataEndIndex)
    {
        this.ReceivedData.SetContents(ReceiveBuffer, dataEndIndex);
        HandleData(this.ReceivedData.Message);
    }
}