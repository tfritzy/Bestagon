using Google.Protobuf.WellKnownTypes;
using System;
using UnityEngine;

public class Client : MonoBehaviour
{
    public TCPConnection TCPConnection;
    public string Id;
    public const string ip = "127.0.0.1";
    public const int port = 8080;

    void Start()
    {
        this.Id = Guid.NewGuid().ToString("N"); ;
        this.TCPConnection = new TCPConnection();
        Connect(ip, port);
    }

    private void Update()
    {
        while(TCPConnection.MessageQueue.Count > 0)
        {
            HandleData(TCPConnection.MessageQueue.First.Value);
            TCPConnection.MessageQueue.RemoveFirst();
        }
    }

    public void Connect(string ip, int port)
    {
        TCPConnection.Connect(ip, port);
    }

    public void HandleData(Any any)
    {
        if (any == null)
        {
            return;
        }

        if (any.Is(Schema.JoinedGame.Descriptor))
        {
            Schema.JoinedGame joinedGame = any.Unpack<Schema.JoinedGame>();
            Debug.Log($"User {joinedGame.Username}");
        }
        else if (any.Is(Schema.BoardState.Descriptor))
        {
            Managers.Board.HandleBoardStateChange(any.Unpack<Schema.BoardState>());
        }
    }

    public void AskForGame()
    {
        Debug.Log("Looking for Game");
        Schema.LookingForGame lookingForGame = new Schema.LookingForGame()
        {
            Username = this.Id,
        };

        TCPConnection.SendMessage(Any.Pack(lookingForGame)).Wait();
    }
}