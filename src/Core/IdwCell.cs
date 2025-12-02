using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core;

internal class IdwCell(
    GridCoordinate position,
    GridNode topLeft,
    GridNode topRight,
    GridNode bottomRight,
    GridNode bottomLeft)
{
    public GridCoordinate Position { get; } = position;
    public GridNode TopLeft { get; } = topLeft;
    public GridNode TopRight { get; } = topRight;
    public GridNode BottomRight { get; } = bottomRight;
    public GridNode BottomLeft { get; } = bottomLeft;
}

internal static class IdwCellExtensions
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