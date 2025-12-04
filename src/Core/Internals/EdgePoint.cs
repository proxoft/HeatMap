namespace Proxoft.Heatmaps.Core.Internals;

internal sealed class EdgePoint(MapPoint mapPoint, bool isVertex) : ValueObject<EdgePoint>
{
    public MapPoint MapPoint { get; } = mapPoint;

    public bool IsVertex { get; } = isVertex;

    public decimal Value => this.MapPoint.Value;

    public Coordinate Coordinate => this.MapPoint.Coordinate;

    protected override bool EqualsCore(EdgePoint other) =>
        other.MapPoint == this.MapPoint
        && other.IsVertex == this.IsVertex;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(this.MapPoint, this.IsVertex);
}