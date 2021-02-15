using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

public class Game
{
    public List<Client> Players;
    public Board Board;
    public const int MaxPlayers = 2;
    public bool IsGameRunning;
    public DateTime LastUpdateTime { get; private set; }

    public Game()
    {
        this.Players = new List<Client>();
        this.Board = new Board();
        this.IsGameRunning = true;
        LastUpdateTime = DateTime.Now;
    }

    public bool IsFull()
    {
        return Players.Count >= MaxPlayers;
    }

    public void Update(DateTime currentTime)
    {
        float deltaTime = (float)(currentTime - LastUpdateTime).TotalMilliseconds / 1000f;

        foreach (Projectile projectile in Board.Projectiles)
        {
            projectile.Position += projectile.Velocity * deltaTime;
        }

        while (Board.NewProjectiles.Count > 0)
        {
            SendMessageToAll(Any.Pack(Board.NewProjectiles.First.Value.BuildCreatedMessage()));
            Board.NewProjectiles.RemoveFirst();
        }

        if (Board.AnyHexagonChanges)
        {
            SendMessageToAll(Any.Pack(Board.GetBoardState()));
        }
    }

    public void SendMessageToAll(Any message)
    {
        foreach (Client player in Players)
        {
            player.SendMessage(message);
        }
    }
}