namespace Proxoft.Heatmaps.Core;

public static class HeatMapFunctions
{
    public static HeatMap CalculateHeatMap(
        this IEnumerable<Item> items,
        IdwSettings settings,
        IIsoLevelCalculator? levelCalculator = null) =>
        CalculateHeatMap([..items], settings, levelCalculator ?? DefaultIsoLevelCalculator.Instance);

    private static HeatMap CalculateHeatMap(Item[] items, IdwSettings settings, IIsoLevelCalculator levelCalculator)
    {
        Bounds bounds = Bounds.FromCoordinates([.. items.Select(i => i.Coordinate)]);
        IdwGrid grid = items.CalculateIdw(bounds, settings);
        decimal[] levels = levelCalculator.CalculateLevels(grid);

        (IsoLine[] isoLines, IsoBand[] isoBands) = grid.CalculateIsoLinesAndIsoBands(levels);

        return new HeatMap(items, bounds, grid, isoLines, isoBands, levels);
    }
}
