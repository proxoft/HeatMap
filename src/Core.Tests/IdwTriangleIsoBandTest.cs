using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core.Tests;

public class IdwTriangleIsoBandTest
{
    [Fact]
    public void OneIsoband_FullRectangle()
    {
        decimal[] levels = [0, 30];
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 26,
            levels
        );

        IsoBand[] isoBands = t.CreateIsoBands(levels);
        SvgExport.SaveToSvg([t], isoBands: isoBands);

        isoBands.Length
            .Should()
            .Be(1);
    }

    [Fact]
    public void TwoIsobands_EdgeB_To_EdgeC()
    {
        decimal[] levels = [0, 15, 30];
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 10,
            v60x0: 20,
            v30x30: 20,
            levels
        );

        IsoBand[] isoBands = t.CreateIsoBands(levels);
        SvgExport.SaveToSvg([t], isoBands: isoBands);

        isoBands.Length
            .Should()
            .Be(2);
    }

    [Fact]
    public void TwoIsobands_C_To_EdgeC()
    {
        decimal[] levels = [0, 15, 30, 45];
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 20,
            v60x0: 40,
            v30x30: 30,
            levels
        );

        IsoBand[] isoBands = t.CreateIsoBands(levels);
        SvgExport.SaveToSvg([t], isoBands: isoBands);

        isoBands.Length
            .Should()
            .Be(2);
    }

    [Fact]
    public void ThreeIsobands_EdgeB_And_EdgeA_To_EdgeC()
    {
        decimal[] levels = [0, 20, 30, 40, 50];
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 25,
            v60x0: 45,
            v30x30: 35,
            levels
        );

        IsoBand[] isoBands = t.CreateIsoBands(levels);
        SvgExport.SaveToSvg([t], isoBands: isoBands);

        isoBands.Length
            .Should()
            .Be(3);
    }

    [Fact]
    public void ThreeIsobands_EdgeB_To_EdgeC()
    {
        decimal[] levels = [0, 20, 30, 40, 50];
        IdwTriangle t = IdwTriangleFactory.Create(
            v0x0: 20,
            v60x0: 50,
            v30x30: 50,
            levels
        );

        IsoBand[] isoBands = t.CreateIsoBands(levels);
        SvgExport.SaveToSvg([t], isoBands: isoBands);

        isoBands.Length
            .Should()
            .Be(3);
    }
}
