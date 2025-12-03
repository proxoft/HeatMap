namespace Proxoft.Heatmaps.Core.Tests;

public class IdwGridTest
{
    [Fact]
    public void Test1()
    {
        IdwGrid grid = IdwGridFactory.Create(
            [
                [10, 20, 30, 40],
                [15, 35, 40, 52],
                [22, 37, 48, 55],
                [25, 29, 42, 41],
            ]
        );

        decimal[] levels = [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60];

        (_, IsoBand[] isoBands) = grid.CalculateIsoLinesAndIsoBands(levels);

        SvgExport.SaveToSvg([], [], isoBands);
    }

    [Fact]
    public void Test2()
    {
        IdwGrid grid = IdwGridFactory.Create(
            [
                [10, 20, 30, 35],
                [15, 35, 44, 36],
                [22, 37, 48, 38],
                [25, 29, 33, 37]
            ]
        );

        decimal[] levels = [10, 20, 30, 40, 50, 60];

        (_, IsoBand[] isoBands) = grid.CalculateIsoLinesAndIsoBands(levels);
        SvgExport.SaveToSvg(triangles: [], isoLines: [], isoBands: isoBands);
    }

    [Fact]
    public void Test3()
    {
        IdwGrid grid = IdwGridFactory.Create(
            [
                [10, 20, 30, 35],
                [15, 35, 44, 36],
                [22, 37, 48, 38],
                [25, 29, 33, 37],
            ]
        );

        decimal[] levels = grid.CalculateLevels();
        Console.WriteLine(string.Join(" ", levels));

        (_, IsoBand[] isoBands) = grid.CalculateIsoLinesAndIsoBands(levels);

        SvgExport.SaveToSvg(isoBands: isoBands);
    }
}
