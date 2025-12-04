using Proxoft.Heatmaps.Core.Exports;

namespace Proxoft.Heatmaps.Svg.Internals;

internal static class IsoBandSvgExport
{
    public static IEnumerable<string> Render(this IsoBand[] isoBands, bool doRender, decimal svgHeight)
    {
        if (!doRender) return [];

        string[] fills = [
            "red",
            "green",
            "blue",
            "orange",
            "yellow",
            "gray",
            "cyan",
            "purple"
        ];

        return isoBands.Select((iso, index) =>
        {
            string fill = fills[index % fills.Length];
            return iso.ToSvgPath(svgHeight, fill);
        });
    }

    public static string ToSvgPath(this IsoBand isoBand, decimal svgHeight, string? fillColor)
    {
        string path = isoBand.ToSvgPath(svgHeight);

        return $"<path d=\"{path}\" fill=\"{fillColor}\" value=\"{isoBand.Value}\" />";
    }

}
