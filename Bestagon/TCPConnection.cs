using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

public class TCPConnection
{
    public TcpClient socket;
    public const int DATA_BUFFER_SIZE = 1024;
    public byte[] ReceiveBuffer;
    public NetworkStream Stream;
    public Packet ReceivedData;

    public TCPConnection()
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = DATA_BUFFER_SIZE,
            SendBufferSize = DATA_BUFFER_SIZE,
        };

        ReceiveBuffer = new byte[DATA_BUFFER_SIZE];

        ReceivedData = new Packet();
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
        byte[] data = this.ReceiveBuffer.Take(dataEndIndex).ToArray();
        this.ReceivedData.Append(data);

        HandleData(data);
        ReceivedData.Reset();
    }

    public bool HandleData(byte[] data)
    {
        return true;
    }
}