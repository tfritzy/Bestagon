using System.Collections.Generic;

public class Board
{
    /// <summary>
    /// All the hexagons present on the board.
    /// Key is playerId. value is dictionary of hexagonId to hexagon.
    /// </summary>
    public Dictionary<int, Dictionary<int, Hexagon>> Hexagons;

    public Board()
    {
        SetupHexagons();
    }

    public Schema.BoardState GetBoardState()
    {
        Schema.BoardState boardState = new Schema.BoardState();

        foreach (int player in Hexagons.Keys)
        {
            Schema.HexagonSet hexagonSet = new Schema.HexagonSet
            {
                PlayerId = player,
            };

            foreach (Hexagon hexagon in Hexagons[player].Values)
            {
                hexagonSet.Hexagons.Add(hexagon.ToContract());
            }
            boardState.PlayerHexagons.Add(hexagonSet);
        }

        return boardState;
    }

    private void SetupHexagons()
    {
        Hexagons = new Dictionary<int, Dictionary<int, Hexagon>>();
        int currentHexagonId = 0;

        Hexagons[0] = new Dictionary<int, Hexagon>();
        Hexagons[1] = new Dictionary<int, Hexagon>();

        // player 0
        for (int y = 0; y < Constants.RowsPerPlayer; y++)
        {
            for (int x = 0; x < Constants.HexagonsPerRow; x++)
            {
                Hexagons[0].Add(currentHexagonId, new Hexagon(GetHexagonPosition(x, y), currentHexagonId));
                currentHexagonId += 1;
            }
        }

        // player 1
        for (int y = Constants.NumVerticalHexagonSlots - Constants.RowsPerPlayer; y < Constants.NumVerticalHexagonSlots; y++)
        {
            for (int x = 0; x < Constants.HexagonsPerRow; x++)
            {
                Hexagons[1].Add(currentHexagonId, new Hexagon(GetHexagonPosition(x, y), currentHexagonId));
                currentHexagonId += 1;
            }
        }
    }

    private Vector2 GetHexagonPosition(int x, int y)
    {
        return new Vector2(
            x * Constants.HorizontalDistanceBetweenHexagons + (x % 2 == 1 ? Constants.HEXAGON_R : 0),
            y * Constants.VerticalDistanceBetweenHexagons + (y % 2 == 1 ? Constants.HEXAGON_r : 0));
    }
}