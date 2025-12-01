namespace Proxoft.Heatmaps.Core;

public static class HeatMapFunctions
{
    public static HeatMap CalculateHeatMap(this IEnumerable<Item> items, IdwSettings settings) =>
        CalculateHeatMap([..items], settings);

    private static HeatMap CalculateHeatMap(Item[] items, IdwSettings settings)
    {
        Bounds bounds = Bounds.FromCoordinates([.. items.Select(i => i.Coordinate)]);
        IdwGrid grid = IdwGridFunctions.CalculateIdw(items, bounds, settings);
        decimal[] levels = grid.CalculateLevels();

        IdwTriangle[] triangles = [
            ..grid.Cells.SelectMany(c => c.SplitToTriangles(levels))
        ];

        IsoLine[] isoLines = triangles.CreateIsoLines();

        return new HeatMap(items, bounds, grid, isoLines, levels);
    }

    private static decimal[] CalculateLevels(this IdwGrid idwGrid)
    {
        if(idwGrid == IdwGrid.None) return [];

        decimal min = idwGrid.Nodes.Select(n => n.MapPoint.Value).Min();
        decimal max = idwGrid.Nodes.Select(n => n.MapPoint.Value).Max();

        int levelCount = 6;
        decimal range = max - min;
        decimal[] levels = [
            .. Enumerable.Range(0, levelCount)
                .Select(i => i == levelCount - 1
                    ? max
                    : min + (range / levelCount) * i
                )
        ];

        return levels;
    }
}
