using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Game
{
    public List<Client> Players;
    public Board Board;
    public const int MaxPlayers = 2;
    public bool IsGameRunning;

    public Game()
    {
        this.Players = new List<Client>();
        this.Board = new Board();
        IsGameRunning = true;
    }

    public bool IsFull()
    {
        return Players.Count >= MaxPlayers;
    }
}