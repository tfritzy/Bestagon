public static class Constants
{
    public const int DEFAULT_BUFFER_SIZE = 1024;
    public const float HEXAGON_r = 0.5f;
    public const float HEXAGON_R = 0.57735026919f;
    public const float ScreenWidth = (NumHorizontalHexagons * 2 + NumHorizontalHexagons - 1) * HEXAGON_R;
    public const float ScreenHeight = (int)((1 / (PixelsPerUnit / 1920)) / (HEXAGON_r * 2));
    public const int NumVerticalHexagonSlots = (int)(ScreenHeight / (HEXAGON_r * 2) + .001);
    public const int NumHorizontalHexagons = 8;
    public const int NumRowsPerPlayer = 8;
    public const float PixelsPerUnit = 1080f / ScreenWidth;
    public const float HorizontalDistanceBetweenHexagons = HEXAGON_r * 2 + HEXAGON_R;
    public const float VerticalDistanceBetweenHexagons = HEXAGON_r * 2;
    public const int TICKS_PER_SECOND = 1;
}