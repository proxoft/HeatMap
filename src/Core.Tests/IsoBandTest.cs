namespace Proxoft.Heatmaps.Core.Tests;

public class IsoBandTest
{
    [Fact]
    public void HavingTwoNeighbouringSimpleIsobandsWithSameLevel_MergeWillProduceSingleIsoband()
    {
        IsoBand i1 = IB(10, "0x0 10x0 10x10");
        IsoBand i2 = IB(10, "0x0 0x10 10x10");

        IsoBand[] merged = [.. new[] { i1, i2 }.MergeNeighboursWithSameLevel()];

        merged
            .Length
            .Should()
            .Be(1);

        merged[0]
            .ShouldHavePolygons("0x0 10x0 10x10 0x10");
    }

    [Fact]
    public void GivenTwoU_ShapedIsoBands_MergeProducesOneIsoBandContainingTwoPolygons()
    {
        IsoBand i1 = IB(10, "0x0 30x0 30x20 20x20 20x10 10x10 10x20 0x20");
        IsoBand i2 = IB(10, "0x40 0x20 10x20 10x30 20x30 20x20 30x20 30x40");

        IsoBand[] merged = [.. new[] { i1, i2 }.MergeNeighboursWithSameLevel()];

        merged
           .Length
           .Should()
           .Be(1);

        merged[0]
            .OuterPolygon
            .ShouldHavePoints("30x20 30x0 0x0 0x20 0x40 30x40");

        merged[0]
            .ShouldHaveInnerPolygons(
                "10x30 10x20 10x10 20x10 20x20 20x30"
            );
    }

    [Fact]

    /*
     _______
     |     _|
     |    |\|  -> empty triangle inside
     |    |_|
     |______|
     */
    public void GivenU_ShapedIsoBand_AndTriangleIsoBand_MergeProducesOneIsoBandContainingTwoPolygons()
    {
        IsoBand i1 = IB(10, "0x0 30x0 30x20 20x20 20x10 10x10 10x20 0x20");
        IsoBand i2 = IB(10, "20x10 20x20 10x20");

        IsoBand[] merged = [.. new[] { i1, i2 }.MergeNeighboursWithSameLevel()];

        merged
           .Length
           .Should()
           .Be(1);

        merged[0]
            .ShouldHavePolygons(
                "10x20 0x20 0x0 30x0 30x20 20x20",
                "10x20 10x10 20x10"
            );
    }

    [Fact]
    public void GivenIsoBandWithHole_AndIsoBandSameAsHole_MergeProducesOneIsoBandContainingOnePolygon()
    {
        IsoBand i1 = IB(10,
            "10x20 10x10 20x10 20x20 20x30 10x30",
            "0x0 30x0 30x20 30x40 0x40 0x20"
        );

        IsoBand i2 = IB(10, "0x0 30x0 30x20 30x40 0x40 0x20");
        IsoBand[] merged = [.. new[] { i1, i2 }.MergeNeighboursWithSameLevel()];

        merged
           .Length
           .Should()
           .Be(1);

        merged[0]
            .ShouldHavePolygons("10x20 10x10 20x10 20x20 20x30 10x30");
    }

    private static IsoBand IB(decimal value, params string[] polygonCoordinates)
    {
        IsoPolygon[] polygons = [.. polygonCoordinates
            .Select(coordinates =>
            {
                Coordinate[] points = [..coordinates.ToCoordinates()];
                return new IsoPolygon(
                    value,
                    points
                );
            })];

        return new IsoBand(value, polygons);
    }
}
