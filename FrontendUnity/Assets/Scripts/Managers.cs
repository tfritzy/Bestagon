using UnityEngine;

public static class Managers
{
    private static Board _board;
    public static Board Board
    {
        get
        {
            if (_board == null)
            {
                _board = GameObject.Find("Board").GetComponent<Board>();
            }

            return _board;
        }
    }

    private static Client client;
    public static Client Client
    {
        get
        {
            if (client == null)
            {
                client = GameObject.Find("Client").GetComponent<Client>();
            }

            return client;
        }
    }

    private static Player player;
    public static Player Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }

            return player;
        }
    }
}