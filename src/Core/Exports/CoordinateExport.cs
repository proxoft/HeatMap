using System.Globalization;

namespace Proxoft.Heatmaps.Core.Exports;

internal static class CoordinateExport
{
    public static string Format(
        this IReadOnlyCollection<Coordinate> coordinates,
        string xyFormat = "({x},{y})",
        string coordinatesSeparator = ",",
        string collectionFormat = "[{collection}]",
        bool addFirstItemAtTheEnd = true
    )
    {
        IReadOnlyCollection<Coordinate> closedCoordinates = [];
        if (addFirstItemAtTheEnd)
        {
            closedCoordinates = [
               ..coordinates,
                coordinates.First()
            ];
        }

        string coords = string.Join(
            coordinatesSeparator,
            [.. closedCoordinates.Select(p => p.Format(xyFormat))]
        );

        return collectionFormat.Replace("{collection}", coords);
    }

    private static string Format(this Coordinate coordinate, string format) =>
        format
            .Replace("{x}", coordinate.X.ToString(CultureInfo.InvariantCulture))
            .Replace("{y}", coordinate.Y.ToString(CultureInfo.InvariantCulture));
}
