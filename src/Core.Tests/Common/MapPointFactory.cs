namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class MapPointFactory
{
    public static MapPoint Value(this string coordinates, decimal value)
    {
        string[] c = coordinates.Split("x");
        return new MapPoint(
            new Coordinate(decimal.Parse(c[0]), decimal.Parse(c[1])),
            value
        );
    }
}
