using System;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

public static class TestObjects
{
    public static Client BuildClient(Server server = null)
    {
        Client client = new Client(Guid.NewGuid().ToString("N"), server ?? new Server(8080, 2));
        return client;
    }

    public static PlayerLookingForGame BuildPlayerLookingForGame(this Client client)
    {
        return new PlayerLookingForGame()
        {
            Username = client.Id,
        };
    }

    public static void SendTestMessage(this Client client, Any any)
    {
        byte[] bytes = any.ToByteArray();
        client.TCPConnection.ReceiveBuffer = bytes;
        client.TCPConnection.ExtractDataFromBuffer(bytes.Length);
    }
}