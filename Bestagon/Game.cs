using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class Game
{
    public List<Client> Players;
    public Board Board;
    public int MaxPlayers;
    public bool IsGameRunning;

    public Game(int maxPlayers)
    {
        this.Players = new List<Client>();
        this.MaxPlayers = maxPlayers;
        this.Board = new Board();
        IsGameRunning = true;
    }

    public bool IsFull()
    {
        return Players.Count >= this.MaxPlayers;
    }
}