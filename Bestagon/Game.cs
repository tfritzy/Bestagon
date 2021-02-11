using System.Collections.Generic;

public class Game
{
    public List<Client> Players;
    private int maxPlayers;
    public Game(int maxPlayers)
    {
        this.Players = new List<Client>();
        this.maxPlayers = maxPlayers;
    }

    public bool IsFull()
    {
        return Players.Count >= this.maxPlayers;
    }
}