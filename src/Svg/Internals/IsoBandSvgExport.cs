using System.Drawing;
using Proxoft.Heatmaps.Core.Exports;

namespace Proxoft.Heatmaps.Svg.Internals;

internal static class IsoBandSvgExport
{
    public static IEnumerable<string> Render(
        this IsoBand[] isoBands,
        decimal[] isoLevels,
        bool doRender,
        ColorPalette colorPalette,
        YScaler yScaler)
    {
        if (!doRender) return [];

        decimal minLevel = isoLevels
            .DefaultIfEmpty(0)
            .Min();

        decimal maxLevel = isoLevels
            .DefaultIfEmpty(0)
            .Max();

        return isoBands.Select((iso, index) =>
        {
            Color color = colorPalette.InterpolateColor(iso.Value, (minimum: minLevel, maximum: maxLevel));
            return iso.ToSvgPath(yScaler, color.ToHex());
        });
    }

    public static string ToSvgPath(this IsoBand isoBand, YScaler yScaler, string? fillColor)
    {
        string path = isoBand.ToSvgPath(yScaler);
        return $"<path d=\"{path}\" fill=\"{fillColor}\" value=\"{isoBand.Value}\" />";
    }

    public static string ToSvgPath(this IsoBand isoBand, YScaler yScaler)
    {
        IEnumerable<Coordinate[]> innerCountours = isoBand.InnerPolygons
            .Select(p => p.Points.CounterClockwise().Select(c => new Coordinate(c.X, yScaler.ToSvgY(c.Y))).ToArray());

        Coordinate[][] contours = [
            [.. isoBand.OuterPolygon.Points.Select(c => new Coordinate(c.X, yScaler.ToSvgY(c.Y)))],
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
