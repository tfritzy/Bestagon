using System.Collections.Generic;

public class Board
{
    public List<Hexagon> Hexagons;

    public Board()
    {
        SetupHexagons();
    }

    private void SetupHexagons()
    {
        Hexagons = new List<Hexagon>();
        int currentHexagonId = 0;

        // player 0
        for (int y = 0; y < Constants.NumRowsPerPlayer; y++)
        {
            for (int x = 0; x < Constants.NumHorizontalHexagons; x++)
            {
                Hexagons.Add(new Hexagon(GetHexagonPosition(x, y), currentHexagonId, 0));
                currentHexagonId += 1;
            }
        }

        // player 1
        for (int y = Constants.NumVerticalHexagonSlots - Constants.NumRowsPerPlayer; y < Constants.NumVerticalHexagonSlots; y++)
        {
            for (int x = 0; x < Constants.NumHorizontalHexagons; x++)
            {
                Hexagons.Add(new Hexagon(GetHexagonPosition(x, y), currentHexagonId, 1));
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