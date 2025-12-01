namespace Proxoft.Heatmaps.Core;

public class GridNode(GridCoordinate gridCoordinate, MapPoint mapPoint) : ValueObject<GridNode>
{
    public GridCoordinate GridCoordinate { get; } = gridCoordinate;

    public MapPoint MapPoint { get; } = mapPoint;

    protected override bool EqualsCore(GridNode other) =>
        other.GridCoordinate == this.GridCoordinate && other.MapPoint == this.MapPoint;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(this.GridCoordinate, this.MapPoint);
}
