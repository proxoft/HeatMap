using System.Text;

namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class SvgExport
{
    public static void ToSvgFile(
        string fileName,
        IdwTriangle[]? idwTriangles = null,
        IsoLine[]? isoLines = null
        )
    {
        string svg = ToSvg(idwTriangles, isoLines);
        File.WriteAllText(fileName.SvgExtension(), svg);
    }

    public static string ToSvg(
        IdwTriangle[]? idwTriangles = null,
        IsoLine[]? isoLines = null
    )
    {
        Bounds bounds = CalculateBounds(idwTriangles: idwTriangles ?? [], isoLines ?? []);

        StringBuilder sb = new();
        foreach (IdwTriangle triangle in idwTriangles ?? [])
        {
            sb.AppendLine(triangle.ToSvg(bounds.Height));
        }

        foreach(IsoLine isoLine in isoLines ?? [])
        {
            sb.AppendLine(isoLine.ToPolyline(bounds.Height));
        }

        string svg = bounds.CreateSvg(sb.ToString(), Padding.Default);
        return svg;
    }

    public static string ToSvg(this IdwTriangle idwTriangle, decimal svgHeight)
    {
        StringBuilder sb = new();

        sb.AppendLine(idwTriangle.A.ToPolyline(svgHeight));
        sb.AppendLine(idwTriangle.B.ToPolyline(svgHeight));
        sb.AppendLine(idwTriangle.C.ToPolyline(svgHeight));

        return sb.ToString();
    }

    public static string ToPolyline(this IdwLine idwLine, decimal svgHeight)
    {
        string points = idwLine
            .Points
            .Select(p => $"{p.Coordinate.X},{svgHeight - p.Coordinate.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        return $"<polyline points=\"{points}\" fill=\"none\" stroke=\"black\"/>";
    }

    public static string ToPolyline(this IsoLine isoLine, decimal svgHeight)
    {
        string points = isoLine
            .Points
            .Select(p => $"{p.X},{svgHeight - p.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        return $"<polyline points=\"{points}\" fill=\"none\" stroke=\"green\"/>";
    }

    private static string CreateSvg(this Bounds bounds, string content, Padding padding)
    {
        StringBuilder sb = new();
        sb.AppendLine($"<svg viewBox=\"{bounds.Left - padding.Left} {bounds.Bottom - padding.Top} {bounds.Width + padding.Horizontal} {bounds.Height + padding.Vertical}\" preserveAspectRatio=\"xMidYMin meet\" xmlns=\"http://www.w3.org/2000/svg\">");
        sb.AppendLine(content);
        sb.AppendLine($"</svg>");
        return sb.ToString();
    }

    private static string SvgExtension(this string fileName) =>
        fileName.EndsWith(".svg", StringComparison.CurrentCultureIgnoreCase)
        ? fileName
        : $"{fileName}.svg";

    private static Bounds CalculateBounds(
        IdwTriangle[] idwTriangles,
        IsoLine[] isoLines)
    {
        Bounds? bounds = null;
        foreach (IdwTriangle triangle in idwTriangles)
        {
            bounds = bounds.MergeOr(triangle.CalculateBounds());
        }

        foreach(IsoLine isoLine in isoLines)
        {
            bounds = bounds.MergeOr(Bounds.FromCoordinates(isoLine.Points));
        }

        return bounds ?? new Bounds(10, 10, 100, 100);
    }
}

internal record Padding(decimal Left, decimal Top, decimal Right, decimal Bottom)
{
    public static readonly Padding Default = new(10, 10, 10, 10);
    public decimal Horizontal => this.Left + this.Right;
    public decimal Vertical => this.Top + this.Bottom;
}

file static class BoundsExtensions
{
    public static Bounds MergeOr(this Bounds? a, Bounds b) =>
        a is null ? b : a.Merge(b);

    public static Bounds AddPadding(this Bounds bounds, decimal left = 10, decimal top = 10, decimal right = 10, decimal bottom = 10) =>
        new(bounds.Left - left, bounds.Top - top, bounds.Right + right, bounds.Bottom + bottom);
}
