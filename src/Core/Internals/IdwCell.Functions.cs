namespace Proxoft.Heatmaps.Core.Internals;

internal static class IdwCellFunctions
{
    extension(IdwCell cell)
    {
        public IEnumerable<IdwTriangle> SplitToTriangles(decimal[] levels)
        {
            MapPoint[] corners = [cell.TopLeft.MapPoint,
                cell.TopRight.MapPoint,
                cell.BottomRight.MapPoint,
                cell.BottomLeft.MapPoint
            ];

            MapPoint center = corners.CalculateCenterPoint();
            yield return IdwTriangle.FromPoints(cell.TopLeft.MapPoint, cell.TopRight.MapPoint, center, levels);
            yield return IdwTriangle.FromPoints(cell.TopRight.MapPoint, cell.BottomRight.MapPoint, center, levels);
            yield return IdwTriangle.FromPoints(cell.BottomRight.MapPoint, cell.BottomLeft.MapPoint, center, levels);
            yield return IdwTriangle.FromPoints(cell.BottomLeft.MapPoint, cell.TopLeft.MapPoint, center, levels);
        }
    }
}