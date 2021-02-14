using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

public static class TestHelpers
{
    public static void ClientSendToServer(this Client client, Any any)
    {
        byte[] bytes = any.ToByteArray();
        client.TCPConnection.ReceiveBuffer = bytes;
        client.TCPConnection.ExtractDataFromBuffer(bytes.Length);
        client.DrainMessageQueue();
    }
}