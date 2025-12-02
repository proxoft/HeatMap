using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core.Tests;

public class IdwCellIsoBandTest
{
    [Fact]
    public void ThreeIsobandsInCell()
    {
        IdwGrid grid = IdwGridFactory.Create(
            [
                [10, 20],
                [30, 40]
            ]
        );
        decimal[] levels = [0, 15, 30, 45];

        IdwTriangle[] triangles = [
            ..grid.Cells.SelectMany(c => c.SplitToTriangles(levels))
        ];

        IsoLine[] isoLines = triangles.CreateIsoLines();
        IsoBand[] isoBands = triangles.CreateIsoBands(levels);

        SvgExport.SaveToSvg(triangles, [], isoBands);

        isoBands.Length
            .Should()
            .Be(3);
    }
}
