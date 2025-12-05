namespace Proxoft.Heatmaps.Svg.Internals;

internal static class MapPointSvgExport
{
    private static string _fill = "lime";

    public static IEnumerable<string> Render(this Item[] items, bool doRender, YScaler yScaler, decimal svgHeight)
    {
        if (!doRender) return [];

        decimal size = svgHeight * 0.01m;

        return items
            .Select(i => i.ToSvg(yScaler, size));
    }

    private static string ToSvg(this Item item, YScaler yScaler, decimal radius)
    {
        decimal y = yScaler.ToSvgY(item.Coordinate.Y);
        return $"<circle cx=\"{item.Coordinate.X}\" cy=\"{y}\" r=\"{radius}\" fill=\"{_fill}\" />";
    }
}
