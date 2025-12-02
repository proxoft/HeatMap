using System.Runtime.CompilerServices;
using Proxoft.Heatmaps.Core.Internals;
using Proxoft.Heatmaps.Core.Tests.Svgs;

namespace Proxoft.Heatmaps.Core.Tests;

public class IdwTriangleIsoLineTest
{
    [Fact]
    public void NoIsoLine_Case1()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 30,
            [30, 40, 50]
        );

        IsoLine[] isoLines = t.CreateIsoLines();
        isoLines.Length
            .Should()
            .Be(0);

        SaveToSvg([t], []);
    }

    [Fact]
    public void OneIsoLine_C_ToEdgeC()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 30,
            [0, 15, 30]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["7.5x7.5".Coord(), "30x0".Coord()], 15));
    }

    [Fact]
    public void OneIsoLine_B_ToEdgeB()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 30,
            v60x0: 40,
            v30x30: 50,
            [0, 40, 80]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["15x15".Coord(), "60x0".Coord()], 40));
    }

    [Fact]
    public void OneIsoLine_A_ToEdgeA()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 40,
            v60x0: 30,
            v30x30: 50,
            [0, 40, 80]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["45x15".Coord(), "0x0".Coord()], 40));
    }

    [Fact]
    public void OneIsoLine_EdgeC_To_EdgeB()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 10,
            [0, 15, 30]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["30x0".Coord(), "45x15".Coord()], 15));
    }

    [Fact]
    public void OneIsoLine_EdgeB_To_EdgeA()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 10,
            v30x30: 30,
            [0, 20, 40]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["45x15".Coord(), "15x15".Coord()], 20));
    }

    [Fact]
    public void OneIsoLine_EdgeB_EdgeC()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 26,
            [0, 15, 30]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(1);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["9.375x9.375".Coord(), "30x0".Coord()], 15));
    }

    [Fact]
    public void TwoIsoLines_EdgeA_To_EdgeC_EdgeB()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 30,
            v60x0: 50,
            v30x30: 5,
            [0, 20, 40, 60]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(2);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["30x0".Coord(), "53.333333333333333333333333334x6.6666666666666666666666666663".Coord()], 40));

        isoLines[1]
            .Should()
            .Be(new IsoLine(["39.999999999999999999999999999x20.000000000000000000000000001".Coord(), "12x12".Coord()], 20));
    }

    [Fact]
    public void TwoIsoLines_EdgeB_To_EdgeC_VertexA()
    {
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 30, // A
            v60x0: 50, // B
            v30x30: 20, // C
            [20, 30, 40, 50]
        );

        IsoLine[] isoLines = t.CreateIsoLines();

        SaveToSvg([t], isoLines);

        isoLines.Length
            .Should()
            .Be(2);

        isoLines[0]
            .Should()
            .Be(new IsoLine(["30x0".Coord(), "50x10".Coord()], 40));

        isoLines[1]
            .Should()
            .Be(new IsoLine(["39.999999999999999999999999999x 20.000000000000000000000000001".Coord(), "0x0".Coord()], 30));
    }

    private static void SaveToSvg(
        IdwTriangle[] triangles,
        IsoLine[] isoLines,
        [CallerMemberName]string? caller = null)
    {
        SvgExport.ToSvgFile(caller ?? "missing-name", idwTriangles: triangles, isoLines: isoLines);
    }
}