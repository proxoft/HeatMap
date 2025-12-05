
namespace Proxoft.Heatmaps.Core.Exports;

internal static class IsoBandExport
{
    public static string ToSvgPath(this IsoBand isoBand, decimal boundsTop)
    {
        IEnumerable<Coordinate[]> innerCountours = isoBand.InnerPolygons
            .Select(p => p.Points.CounterClockwise().Select(c => new Coordinate(c.X, boundsTop - c.Y)).ToArray());

        Coordinate[][] contours = [
            [.. isoBand.OuterPolygon.Points.Select(c => new Coordinate(c.X, boundsTop - c.Y))],
            ..innerCountours
        ];

        IEnumerable<string> x = [..
            contours.Select(
            contour => contour.Format(
                xyFormat: "L {x},{y}",
                coordinatesSeparator: " ",
                collectionFormat: "{collection} z")
            )
            .Select(path => "M" + path[1..])
        ];

        return string.Join(" ", x);
    }
}
