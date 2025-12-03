namespace Proxoft.Heatmaps.Core.Internals;

internal static class IdwTriangleFunctions
{
    extension(IdwTriangle triangle)
    {
        public Bounds CalculateBounds() =>
            Bounds.FromCoordinates([
                ..triangle.A.Points.Select(p => p.Coordinate),
                ..triangle.B.Points.Select(p => p.Coordinate),
                ..triangle.C.Points.Select(p => p.Coordinate),
           ]);
    }
}

internal static class IdwTriangleIsoLineFunctions
{
    public static IsoLine[] CreateIsoLines(this IEnumerable<IdwTriangle> triangles) =>
        [
            ..triangles
                .SelectMany(t => t.CreateIsoLines()) // https://en.wikipedia.org/wiki/Marching_squares
                .Defragment()
        ];

    public static IsoLine[] CreateIsoLines(this IdwTriangle triangle)
    {
        MapPoint[] a = [.. triangle.A.Points.ExceptOfFirstAndLast()];
        MapPoint[] b = [.. triangle.B.Points.ExceptOfFirstAndLast()];
        MapPoint[] c = [.. triangle.C.Points.ExceptOfFirstAndLast()];

        return [
            ..a.FindIsoLines(triangle.B.Points),
            ..b.FindIsoLines(triangle.C.Points),
            ..c.FindIsoLines(triangle.A.Points)
        ];
    }

    private static IEnumerable<IsoLine> FindIsoLines(this IEnumerable<MapPoint> origins, IEnumerable<MapPoint> targets) =>
        origins
            .Select(a =>
            {
                MapPoint? b = targets.SingleOrDefault(p => p.Value == a.Value);
                if (b is null)
                {
                    return Array.Empty<MapPoint>();
                }

                return [a, b];
            })
            .Where(ps => ps.Length > 0)
            .Select(ps => (value: ps.First().Value, points: ps.Select(p => p.Coordinate).ToArray()))
            .Select(data => new IsoLine(data.points, data.value))
            ;
}

internal static class IdwTriangleIsoBandFunctions
{
    public static IsoBand[] CreateIsoBands(this IEnumerable<IdwTriangle> triangles, decimal[] levels) =>
        [.. triangles
            .SelectMany(t => CreateIsoBands(t, levels))
            .MergeNeighboursWithSameLevel()
        ];

    public static IsoBand[] CreateIsoBands(this IdwTriangle triangle, decimal[] levels)
    {
        IsoBand[] isoBands = [
            ..triangle
                .GetEdgePoints()
                .FindAtomicIsoBands(levels)
                .MergeNeighboursWithSameLevel()
        ];

        return isoBands;
    }

    private static IEnumerable<IsoBand> FindAtomicIsoBands(
        this EdgePoint[] edgePoints,
        decimal[] levels)
    {
        return edgePoints
            .FindAtomicIsoPolygons(levels)
            .Select(polygon => IsoBand.From(polygon));
    }

    private static EdgePoint[] GetEdgePoints(this IdwTriangle triangle)
    {
        // sort the edge points so that we start processing edges with most of the the points
        int ri = triangle.CalculateEdgePointProcessingOrder();
        IEnumerable<EdgePoint> a = triangle.A.Points
            .SkipLast(1)
            .Select((p, i) => new EdgePoint(p, i == 0));

        IEnumerable<EdgePoint> b = triangle.B.Points
            .SkipLast(1)
            .Select((p, i) => new EdgePoint(p, i == 0));

        IEnumerable<EdgePoint> c = triangle.C.Points
            .SkipLast(1)
            .Select((p, i) => new EdgePoint(p, i == 0));

        return ri switch
        {
            0 => [.. c, .. a, .. b],
            1 => [.. a, .. b, .. c],
            _ => [.. b, .. c, .. a]
        };
    }

    private static int CalculateEdgePointProcessingOrder(this IdwTriangle triangle)
    {
        int a = triangle.A.Points.Count();
        int b = triangle.B.Points.Count();
        int c = triangle.C.Points.Count();

        if (a <= b && a <= c)
        {
            return 0;
        }

        if (b <= a && b <= c)
        {
            return 1;
        }

        return 2;
    }
}

internal static class IdwTriangleCommonFunctions
{
    public static IEnumerable<T> ExceptOfFirstAndLast<T>(this IEnumerable<T> source) =>
        source.Skip(1).SkipLast(1);
}
