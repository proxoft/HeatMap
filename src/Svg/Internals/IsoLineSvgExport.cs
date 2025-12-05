using System.Drawing;

namespace Proxoft.Heatmaps.Svg.Internals;

internal static class IsoLineSvgExport
{
    public static IEnumerable<string> Render(
       this IsoLine[] isoLines,
       bool doRender,
       YScaler yScaler,
       decimal svgHeight)
    {
        if (!doRender) return [];

        decimal minLevel = isoLines.Select(i => i.Value)
            .DefaultIfEmpty(0)
            .Min();

        decimal maxLevel = isoLines.Select(i => i.Value)
            .DefaultIfEmpty(0)
            .Max();

        decimal strokeWidth = svgHeight * 0.005m;

        return isoLines.Select(line =>
        {
            Color color = ColorPalette.BlueRed.InterpolateColor(line.Value, (minimum: minLevel, maximum: maxLevel));
            return line.ToPolyline(color, yScaler, strokeWidth);
        });
    }

    private static string ToPolyline(this IsoLine isoLine, Color color, YScaler yScaler, decimal strokeWidth)
    {
        string points = isoLine
            .Points
            .Select(p => $"{p.X},{yScaler.ToSvgY(p.Y)}")
            .Aggregate((acc, next) => $"{acc} {next}");

        //string original = isoLine
        //    .Points
        //    .Select(p => $"{p.X},{p.Y}")
        //    .Aggregate((acc, next) => $"{acc} {next}");

        return $"<polyline points=\"{points}\" value=\"{isoLine.Value}\" fill=\"none\" stroke=\"{color.ToHex()}\" stroke-width=\"{strokeWidth}\" />";
    }
}
