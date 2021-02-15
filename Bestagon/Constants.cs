public static class Constants
{
    public const int DEFAULT_BUFFER_SIZE = 16000;
    public const float HEXAGON_r = 0.5f;
    public const float HEXAGON_R = 0.57735026919f;
    public const float ScreenWidth = (HexagonsPerRow * 2 + HexagonsPerRow - 1) * HEXAGON_R;
    public const float ScreenHeight = ScreenWidth * 1.777f;
    public const int NumVerticalHexagonSlots = (int)(ScreenHeight / (HEXAGON_r * 4) + .001);
    public const int HexagonsPerRow = 14;
    public const int RowsPerPlayer = 6;
    public const float HorizontalDistanceBetweenHexagons = HEXAGON_R + HEXAGON_r / 2;
    public const float VerticalDistanceBetweenHexagons = HEXAGON_r * 2;
    public const int MS_Between_Updates = 1000;
    public static readonly float[] WallPositions = new float[] { 10.22f, 6.94f, -11.99f, -6.27f }; // In order Top, right, bottom, left
}