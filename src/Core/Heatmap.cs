namespace Proxoft.Heatmaps.Core;

// To verify polygons:
// https://www.desmos.com/calculator/mhq4hsncnh
public record HeatMap(
    Item[] Items,
    Bounds Bounds,
    IdwGrid IdwGrid,
    IsoLine[] IsoLines,
    decimal[] Levels
)
{
    public static readonly HeatMap None = new([], Bounds.None, IdwGrid.None, [], []);
}
