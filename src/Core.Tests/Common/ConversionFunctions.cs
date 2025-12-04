namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class ConversionFunctions
{
    public static IEnumerable<Coordinate> ToCoordinates(this string coordinatesList, string separator = " ") =>
        coordinatesList
            .Split(separator)
            .ToCoordinates();

    public static IEnumerable<Coordinate> ToCoordinates(this IEnumerable<string> coordinates) =>
        coordinates.Select(c => c.Coord());

    public static Coordinate Coord(this string coordinates) =>
        new(
            decimal.Parse(coordinates.Split('x')[0]),
            decimal.Parse(coordinates.Split('x')[1])
        );
}
