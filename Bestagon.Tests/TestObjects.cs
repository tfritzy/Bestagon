using System;
using System.Net.Sockets;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Moq;

public static class TestObjects
{
    public static Client BuildClient(Server server = null)
    {
        Client client = new Client(Guid.NewGuid().ToString("N"), server ?? new Server(8080, 2));
        return client;
    }

    public static Schema.LookingForGame BuildLookingForGame(this Client client)
    {
        return new Schema.LookingForGame()
        {
            Username = client.Id,
        };
    }
}