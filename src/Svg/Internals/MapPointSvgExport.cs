namespace Proxoft.Heatmaps.Svg.Internals;

internal static class MapPointSvgExport
{
    private static string _fill = "lime";

    public static IEnumerable<string> Render(this Item[] items, bool doRender, decimal svgHeight)
    {
        if (!doRender) return [];

        decimal size = svgHeight * 0.01m;

        return items
            .Select(i => i.ToSvg(svgHeight, size));
    }

    private static string ToSvg(this Item item, decimal svgHeight, decimal radius) =>
        $"<circle cx=\"{item.Coordinate.X}\" cy=\"{svgHeight - item.Coordinate.Y}\" r=\"{radius}\" fill=\"{_fill}\" />";
}
