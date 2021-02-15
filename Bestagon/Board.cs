using System.Collections.Generic;

public class Board
{
    /// <summary>
    /// All the hexagons present on the board.
    /// Key is hexagonId value is hexagon.
    /// </summary>
    public Dictionary<int, Hexagon> Hexagons;
    public List<Projectile> Projectiles;
    public LinkedList<Projectile> NewProjectiles;

    private List<int> unsentChanges;
    private int projectileIdGenerator;

    public Board()
    {
        this.Projectiles = new List<Projectile>();
        this.NewProjectiles = new LinkedList<Projectile>();

        SetupHexagons();
    }

    public bool AnyHexagonChanges => unsentChanges.Count > 0;

    public void CreateProjectile(int playerId, Vector2 position, Vector2 velocity, int projectileType)
    {
        Projectile projectile = new BouncingBall(projectileIdGenerator, position, velocity);
        projectileIdGenerator += 1;
        this.Projectiles.Add(projectile);
        NewProjectiles.AddLast(projectile);
    }

    public void DestroyHexagon(int id)
    {
        if (Hexagons.ContainsKey(id))
        {
            if (Hexagons[id].IsDestroyed == false)
            {
                Hexagons[id].Destroy();
                unsentChanges.Add(id);
            }

            return;
        }

        throw new System.ArgumentException($"Hexagon with id {id} does not exist");
    }

    public Schema.BoardState GetBoardState()
    {
        Schema.BoardState boardState = new Schema.BoardState();

        for (int i = 0; i < Game.MaxPlayers; i++)
        {
            boardState.HexagonSets.Add(new Schema.HexagonSet { PlayerId = i });
        }

        foreach (int hexagonId in unsentChanges)
        {
            boardState.HexagonSets[Hexagons[hexagonId].PlayerId].Hexagons.Add(Hexagons[hexagonId].ToContract());
        }

        unsentChanges = new List<int>();

        return boardState;
    }

    private void SetupHexagons()
    {
        Hexagons = new Dictionary<int, Hexagon>();
        unsentChanges = new List<int>();

        int currentHexagonId = 0;

        Hexagons = new Dictionary<int, Hexagon>();

        // player 0
        for (int y = 0; y < Constants.RowsPerPlayer; y++)
        {
            for (int x = 0; x < Constants.HexagonsPerRow; x++)
            {
                Hexagon hexagon = new Hexagon(GetHexagonPosition(x, y), currentHexagonId, 0);
                Hexagons.Add(currentHexagonId, hexagon);
                unsentChanges.Add(hexagon.Id);
                currentHexagonId += 1;
            }
        }

        // player 1
        for (int y = Constants.NumVerticalHexagonSlots - Constants.RowsPerPlayer; y < Constants.NumVerticalHexagonSlots; y++)
        {
            for (int x = 0; x < Constants.HexagonsPerRow; x++)
            {
                Hexagon hexagon = new Hexagon(GetHexagonPosition(x, y), currentHexagonId, 1);
                Hexagons.Add(currentHexagonId, hexagon);
                unsentChanges.Add(hexagon.Id);
                currentHexagonId += 1;
            }
        }
    }

    private Vector2 GetHexagonPosition(int x, int y)
    {
        float xF = x * Constants.HorizontalDistanceBetweenHexagons;
        float yF = y * Constants.VerticalDistanceBetweenHexagons + (x % 2 == 1 ? Constants.HEXAGON_r : 0);
        return new Vector2(xF, yF);
    }
}