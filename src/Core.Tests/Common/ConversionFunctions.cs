namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class ConversionFunctions
{
    public static Coordinate Coord(this string coordinates) =>
        new(
            decimal.Parse(coordinates.Split('x')[0]),
            decimal.Parse(coordinates.Split('x')[1])
        );
}
