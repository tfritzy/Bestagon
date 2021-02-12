public class Hexagon
{
    public Vector2 Position;
    public int Id;
    public int Player;

    public Hexagon(Vector2 position, int id, int player)
    {
        this.Position = position;
        this.Id = id;
        this.Player = player;
    }
}