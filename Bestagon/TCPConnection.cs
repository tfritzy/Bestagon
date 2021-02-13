using System;
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

    public void Connect(TcpClient client)
    {
        socket = client;
        socket.ReceiveBufferSize = Constants.DEFAULT_BUFFER_SIZE;
        socket.SendBufferSize = Constants.DEFAULT_BUFFER_SIZE;

        Stream = socket.GetStream();

        Stream.BeginRead(receiveBuffer, 0, Constants.DEFAULT_BUFFER_SIZE, ReceiveCallback, null);
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