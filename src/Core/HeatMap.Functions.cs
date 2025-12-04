namespace Proxoft.Heatmaps.Core;

public static class HeatMapFunctions
{
    public static HeatMap CalculateHeatMap(this IEnumerable<Item> items, IdwSettings settings) =>
        CalculateHeatMap([..items], settings);

    private static HeatMap CalculateHeatMap(Item[] items, IdwSettings settings)
    {
        Bounds bounds = Bounds.FromCoordinates([.. items.Select(i => i.Coordinate)]);
        IdwGrid grid = items.CalculateIdw(bounds, settings);
        decimal[] levels = grid.CalculateLevels();

        (IsoLine[] isoLines, IsoBand[] isoBands) = grid.CalculateIsoLinesAndIsoBands(levels);

        return new HeatMap(items, bounds, grid, isoLines, isoBands, levels);
    }
}
