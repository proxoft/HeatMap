namespace Proxoft.Heatmaps.Core.Tests;

public class HeatMapTest
{
    [Fact]
    public void Example1()
    {
        Item[] items = [
            new((10, 20), 234),
            new((18, 30), 526),
            new((25, 2), 112),
            new((29, 15), 890),
            new((15, 18), 264),
            new((5, 5), 457),
            new((29, 2), 698),
            new((4, 18), 348),
        ];

        HeatMap heatMap = items.CalculateHeatMap(IdwSettings.Default);
        SvgExport.SaveToSvg(triangles: [], isoLines: [], isoBands: heatMap.IsoBands);
    }
}
