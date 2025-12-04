namespace Proxoft.Heatmaps.Core.Internals;

internal static class EdgePointFunctions
{
    public static IsoPolygon ToIsoPolygon(this IReadOnlyCollection<EdgePoint> points, LevelRange[] levels)
    {
        LevelRange level = points.Select(p => p.Value).GetLevelRange(levels);
        return new IsoPolygon(level.From, [.. points.Select(p => p.Coordinate)]);
    }

    public static IEnumerable<IsoPolygon> FindAtomicIsoPolygons(this EdgePoint[] edgePoints, decimal[] levels)
    {
        LevelRange[] levelRanges = [.. LevelRange.FromLevels(levels)];
        Index left = 1;
        Index right = ^1;

        IsoPolygon isoPolygon = new[] { edgePoints[left], edgePoints[0], edgePoints[right] }.ToIsoPolygon(levelRanges);
        yield return isoPolygon;

        while (!left.IsSameOrNeighbour(right, edgePoints.Length))
        {
            (isoPolygon, left, right) = edgePoints.FindAtomicIsoBands(left, right, levelRanges);
            yield return isoPolygon;
        }
    }

    public static (IsoPolygon, Index left, Index right) FindAtomicIsoBands(
        this EdgePoint[] edgePoints,
        Index startLeft,
        Index startRight,
        LevelRange[] levels)
    {
        Index left = startLeft;
        Index right = startRight;

        EdgePoint currentLeft = edgePoints[left];
        EdgePoint currentRight = edgePoints[right];

        List<EdgePoint> points = new()
        {
            currentLeft,
            currentRight
        };

        (currentLeft, left) = edgePoints.Next(left);
        points.AddLeft(currentLeft);

        if (left.IsSameOrNeighbour(right, edgePoints.Length))
        {
            return (points.ToIsoPolygon(levels), left, right);
        }

        (currentRight, right) = edgePoints.Next(right);
        points.AddRight(currentRight);

        if (left.IsSameOrNeighbour(right, edgePoints.Length)
            || currentLeft.Value == currentRight.Value)
        {
            return (points.ToIsoPolygon(levels), left, right);
        }

        if (currentLeft.IsVertex)
        {
            (currentLeft, left) = edgePoints.Next(left);
            points.AddLeft(currentLeft);
        }

        if (left.IsSameOrNeighbour(right, edgePoints.Length)
            || currentLeft.Value == currentRight.Value)
        {
            return (points.ToIsoPolygon(levels), left, right);
        }

        if (currentRight.IsVertex)
        {
            (currentRight, right) = edgePoints.Next(right);
            points.AddRight(currentRight);
        }

        return (points.ToIsoPolygon(levels), left, right);
    }

    private static (EdgePoint p, Index nextIndex) Next(this EdgePoint[] edgePoints, Index currentLeft)
    {
        Index n = currentLeft.Next();
        return (edgePoints[n], n);
    }
}
