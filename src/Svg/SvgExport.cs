using System.Text;
using Proxoft.Heatmaps.Svg.Internals;

namespace Proxoft.Heatmaps.Svg;

public static class SvgExport
{
    public static string ToSvg(this HeatMap heatMap, SvgOptions options)
    {
        Bounds bounds = heatMap.IdwGrid.CalculateBounds();

        string[] content = [
            ..heatMap.IsoBands.Render(options.IsoBands, bounds.Height),
            ..heatMap.Items.Render(options.Items, bounds.Height)
        ];

        string svg = content.CreateSvg(bounds, Padding.Default);
        return svg;
    }

    private static string CreateSvg(this string[] content, Bounds bounds, Padding padding)
    {
        StringBuilder sb = new();
        sb.AppendLine($"<svg viewBox=\"{bounds.Left - padding.Left} {bounds.Bottom - padding.Top} {bounds.Width + padding.Horizontal} {bounds.Height + padding.Vertical}\" preserveAspectRatio=\"xMidYMin meet\" xmlns=\"http://www.w3.org/2000/svg\">");
        foreach (string item in content)
        {
            sb.AppendLine(item);
        }
        sb.AppendLine($"</svg>");
        return sb.ToString();
    }
}

