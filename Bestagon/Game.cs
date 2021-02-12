using System.Collections.Generic;

public class Game
{
    public List<Client> Players;
    public Board Board;
    public int MaxPlayers;

    public Game(int maxPlayers)
    {
        this.Players = new List<Client>();
        this.MaxPlayers = maxPlayers;
        this.Board = new Board();
    }

    public bool IsFull()
    {
        return Players.Count >= this.MaxPlayers;
    }
}