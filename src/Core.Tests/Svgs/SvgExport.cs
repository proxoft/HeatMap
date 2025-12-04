using System.Runtime.CompilerServices;
using System.Text;
using Proxoft.Heatmaps.Core;
using Proxoft.Heatmaps.Core.Exports;
using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core.Tests.Svgs;

internal static class SvgExport
{
    public static void SaveToSvg(
        IdwTriangle[]? triangles = null,
        IsoLine[]? isoLines = null,
        IsoBand[]? isoBands = null,
        [CallerFilePath]string callerFilePath = "",
        [CallerMemberName] string caller = "")
    {
        string callerTypeName = Path.GetFileNameWithoutExtension(callerFilePath);
        SvgExport.ToSvgFile(
            $"{callerTypeName}_{caller}",
            idwTriangles: triangles,
            isoLines: isoLines,
            isoBands: isoBands
        );
    }

    public static void ToSvgFile(
        string fileName,
        IdwTriangle[]? idwTriangles = null,
        IsoLine[]? isoLines = null,
        IsoBand[]? isoBands = null
        )
    {
        string svg = ToSvg(idwTriangles, isoLines, isoBands);
        File.WriteAllText(fileName.SvgExtension(), svg);
    }

    public static string ToSvg(
        IdwTriangle[]? idwTriangles = null,
        IsoLine[]? isoLines = null,
        IsoBand[]? isoBands = null
    )
    {
        Bounds bounds = CalculateBounds(idwTriangles: idwTriangles ?? [], isoLines ?? [], isoBands ?? []);

        StringBuilder sb = new();
        foreach (IdwTriangle triangle in idwTriangles ?? [])
        {
            sb.AppendLine(triangle.ToSvg(bounds.Height));
        }

        foreach(IsoLine isoLine in isoLines ?? [])
        {
            sb.AppendLine(isoLine.ToPolyline(bounds.Height));
        }

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

        int fillIndex = 0;
        foreach(IsoBand isoBand in isoBands ?? [])
        {
            sb.AppendLine(isoBand.ToSvgPath(bounds.Height, fills[fillIndex]));
            fillIndex = (fillIndex + 1) % fills.Length;
        }

        string svg = bounds.CreateSvg(sb.ToString(), Padding.Default);
        return svg;
    }

    public static string ToSvg(this IdwTriangle idwTriangle, decimal svgHeight)
    {
        StringBuilder sb = new();

        sb.AppendLine(idwTriangle.A.ToLine(svgHeight));
        sb.AppendLine(idwTriangle.B.ToLine(svgHeight));
        sb.AppendLine(idwTriangle.C.ToLine(svgHeight));

        return sb.ToString();
    }

    public static string ToLine(this IdwLine idwLine, decimal svgHeight)
    {
        Coordinate s = idwLine.Points.First().Coordinate;
        Coordinate e = idwLine.Points.Last().Coordinate;

        return $"<line x1=\"{s.X}\" y1=\"{svgHeight - s.Y}\" x2=\"{e.X}\" y2=\"{svgHeight - e.Y}\" stroke=\"black\" />";
    }

    public static string ToPolyline(this IdwLine idwLine, decimal svgHeight)
    {
        string points = idwLine
            .Points
            .Select(p => $"{p.Coordinate.X},{svgHeight - p.Coordinate.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        string original = idwLine
            .Points
            .Select(p => $"{p.Coordinate.X},{p.Coordinate.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        return $"<polyline points=\"{points}\" original=\"{original}\" fill=\"none\" stroke=\"black\" />";
    }

    public static string ToPolyline(this IsoLine isoLine, decimal svgHeight)
    {
        string points = isoLine
            .Points
            .Select(p => $"{p.X},{svgHeight - p.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        string original = isoLine
            .Points
            .Select(p => $"{p.X},{p.Y}")
            .Aggregate((acc, next) => $"{acc} {next}");

        return $"<polyline points=\"{points}\" original=\"{original}\" fill=\"none\" stroke=\"green\"/>";
    }

    public static string ToSvgPath(this IsoBand isoBand, decimal svgHeight, string? fillColor)
    {
        string path = isoBand.ToSvgPath(svgHeight);

        return $"<path d=\"{path}\" fill=\"{fillColor}\" value=\"{isoBand.Value}\" />";
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

    private static string ToSvgPathPoints(this IEnumerable<Coordinate> coordinates, decimal svgHeight) =>
        coordinates
            .Select(c => c.SvgPathPoint(svgHeight))
            .Aggregate((acc, p) => $"{acc} {p}");

    private static string SvgPathPoint(this Coordinate coordinate, decimal svgHeight) =>
        $"{coordinate.X},{svgHeight - coordinate.Y}";

    private static Bounds CalculateBounds(
        IdwTriangle[] idwTriangles,
        IsoLine[] isoLines,
        IsoBand[] isoBands)
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

        foreach (IsoPolygon isoPolygon in isoBands.SelectMany(ib => ib.IsoPolygons))
        {
            bounds = bounds.MergeOr(isoPolygon.CalculateBounds());
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
}
