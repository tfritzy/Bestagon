public class Collisions
{
    public static void FindIntersection(
        Vector2 v1Start, Vector2 v1End, Vector2 v2Start, Vector2 v2End,
        out bool lines_intersect, out bool segments_intersect,
        out Vector2 intersection)
    {
        // Get the segments' parameters.
        float dx12 = v1End.X - v1Start.X;
        float dy12 = v1End.Y - v1Start.Y;
        float dx34 = v2End.X - v2Start.X;
        float dy34 = v2End.Y - v2Start.Y;

        // Solve for t1 and t2
        float denominator = (dy12 * dx34 - dx12 * dy34);

        float t1 =
            ((v1Start.X - v2Start.X) * dy34 + (v2Start.Y - v1Start.Y) * dx34)
                / denominator;
        if (float.IsInfinity(t1))
        {
            // The lines are parallel (or close enough to it).
            lines_intersect = false;
            segments_intersect = false;
            intersection = new Vector2(float.NaN, float.NaN);
            return;
        }
        lines_intersect = true;

        float t2 =
            ((v2Start.X - v1Start.X) * dy12 + (v1Start.Y - v2Start.Y) * dx12)
                / -denominator;

        // Find the point of intersection.
        intersection = new Vector2(v1Start.X + dx12 * t1, v1Start.Y + dy12 * t1);

        // The segments intersect if t1 and t2 are between 0 and 1.
        segments_intersect =
            ((t1 >= 0) && (t1 <= 1) &&
             (t2 >= 0) && (t2 <= 1));
    }
}