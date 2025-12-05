using System.Text;
using Proxoft.Heatmaps.Svg.Internals;

namespace Proxoft.Heatmaps.Svg;

public static class SvgExport
{
    public static string ToSvg(this HeatMap heatMap, SvgOptions options)
    {
        Bounds bounds = heatMap.IdwGrid.CalculateBounds();
        YScaler yScaler = new(bounds.Top);

        string[] content = [
            ..heatMap.IsoBands.Render(options.IsoBands, yScaler),
            ..heatMap.Items.Render(options.Items, yScaler, bounds.Height)
        ];

        Padding padding = bounds.CalculatePadding();
        string svg = content.CreateSvg(bounds, padding);
        return svg;
    }

    private static string CreateSvg(this string[] content, Bounds bounds, Padding padding)
    {
        StringBuilder sb = new();
        sb.AppendLine($"<svg viewBox=\"{bounds.Left - padding.Left} {0 - padding.Top} {bounds.Width + padding.Horizontal} {bounds.Height + padding.Vertical}\" preserveAspectRatio=\"xMidYMin meet\" xmlns=\"http://www.w3.org/2000/svg\">");
        foreach (string item in content)
        {
            sb.AppendLine(item);
        }
        sb.AppendLine($"</svg>");
        return sb.ToString();
    }

    private static Padding CalculatePadding(this Bounds bounds)
    {
        decimal horizontal = Math.Min(10, bounds.Width * 0.1m);
        decimal vertical = Math.Min(10, bounds.Height * 0.1m);

        return new Padding(horizontal / 2, vertical / 2, horizontal / 2, vertical / 2);
    }
}

