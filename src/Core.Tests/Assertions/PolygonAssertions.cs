namespace Proxoft.Heatmaps.Core.Tests.Assertions;

internal static class PolygonAssertions
{
    public static void ShouldHavePolygons(this IsoBand isoBand, params string[] polygonCoordinates)
    {
        isoBand.IsoPolygons.Length
            .Should()
            .Be(polygonCoordinates.Length);

        int i = 0;
        foreach (IsoPolygon p in isoBand.IsoPolygons)
        {
            p.ShouldHavePoints(polygonCoordinates[i]);
            i++;
        }
    }

    public static void ShouldHaveInnerPolygons(this IsoBand isoBand,
        params string[] polygonCoordinates)
    {
        isoBand.InnerPolygons.Count()
            .Should()
            .Be(polygonCoordinates.Length);

        int i = 0;
        foreach (IsoPolygon p in isoBand.InnerPolygons)
        {
            p.ShouldHavePoints(polygonCoordinates[i]);
            i++;
        }
    }

    public static void ShouldHavePoints(this IsoBand isoBand, params MapPoint[] points)
    {
        isoBand.OuterPolygon.Points.Clockwise()
            .Should()
            .BeEquivalentTo(points.Select(p => p.Coordinate));
    }

    public static void ShouldHavePoints(
        this IsoPolygon polygon,
        string coordinates)
    {
        Coordinate[] expected = [..coordinates.ToCoordinates()];

        polygon.Points
            .Should()
            .BeEquivalentTo(expected);
    }
}
