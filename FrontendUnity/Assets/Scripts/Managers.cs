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
}