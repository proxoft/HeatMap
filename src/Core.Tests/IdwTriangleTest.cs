using System.Runtime.CompilerServices;
using Proxoft.Heatmaps.Core.Tests.Common;

namespace Proxoft.Heatmaps.Core.Tests;


public class IdwTriangleTest
{
    [Fact]
    public void NoIsoLine_Case1()
    {
        IdwTriangle t = Factory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 30
        );

        IsoLine[] isoLines = t.CreateIsoLines();
        isoLines.Length
            .Should()
            .Be(0);

        SaveToSvg([t], []);
    }

    [Fact]
    public void OneIsoLine_BetweenEdges1()
    {
        IdwTriangle t = Factory.Create(
            v0x0: 10,
            v30x0: 15,
            v60x0: 20,
            v30x30: 30,
            v20x20: 15
        );

        IsoLine[] isoLines = t.CreateIsoLines();
        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["20x20".Coord(), "30x0".Coord()], 15));

        SaveToSvg([t], isoLines);
    }

    [Fact]
    public void OneIsoLine_BetweenEdges2()
    {
        IdwTriangle t = Factory.Create(
            v0x0: 10,
            v15x0: 15,
            v60x0: 20,
            v30x30: 20,
            v20x20: 15
        );

        IsoLine[] isoLines = t.CreateIsoLines();
        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["20x20".Coord(), "15x0".Coord()], 15));

        SaveToSvg([t], isoLines);
    }

    [Fact]
    public void OneIsoLine_EdgeCase1()
    {
        IdwTriangle t = Factory.Create(
            v0x0: 10,
            v15x0: 15,
            v60x0: 20,
            v30x30: 15
        );

        IsoLine[] isoLines = t.CreateIsoLines();
        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["15x0".Coord(), "30x30".Coord()], 15));

        SaveToSvg([t], isoLines);
    }

    private static void SaveToSvg(
        IdwTriangle[] triangles,
        IsoLine[] isoLines,
        [CallerMemberName]string? caller = null)
    {
        SvgExport.ToSvgFile(caller ?? "missing-name", idwTriangles: triangles, isoLines: isoLines);
    }
}

file static class Factory
{
    public static IdwTriangle Create(
        decimal? v0x0 = null,
        decimal? v15x0 = null,
        decimal? v30x0 = null,
        decimal? v45x0 = null,
        decimal? v60x0 = null,
        decimal? v50x10 = null,
        decimal? v10x20 = null,
        decimal? v30x30 = null,
        decimal? v20x20 = null,
        decimal? v10x10 = null
    )
    {
        IdwLine[] lines = [.. CreateLines(
            v0x0,
            v15x0,
            v30x0,
            v45x0,
            v60x0,
            v50x10,
            v10x20,
            v30x30,
            v20x20,
            v10x10
        )];

        return new IdwTriangle(lines[0], lines[1], lines[2]);
    }

    private static IEnumerable<IdwLine> CreateLines(
        decimal? v0x0,
        decimal? v15x0,
        decimal? v30x0,
        decimal? v45x0,
        decimal? v60x0,
        decimal? v50x10,
        decimal? v40x20,
        decimal? v30x30,
        decimal? v20x20,
        decimal? v10x10)
    {
        yield return new[] { ("0x0", v0x0), ("15x0", v15x0), ("30x0", v30x0), ("45x0", v45x0), ("60x0", v60x0) }.CreateLine();
        yield return new[] { ("60x0", v60x0), ("50x10", v50x10), ("40x20", v40x20), ("30x30", v30x30) }.CreateLine();
        yield return new[] { ("30x30", v30x30), ("20x20", v20x20), ("10x10", v10x10), ("0x0", v0x0) }.CreateLine();
    }

    private static IdwLine CreateLine(this IEnumerable<(string, decimal?)> coordinates) =>
        new([.. coordinates.ToMapPoints()]);

    private static IEnumerable<MapPoint> ToMapPoints(this IEnumerable<(string coordinate, decimal? value)> pairs) =>
        pairs
           .Where(p => p.value is not null)
           .Select(p => p.coordinate.Value(p.value!.Value));
}
