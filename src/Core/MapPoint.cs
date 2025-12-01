namespace Proxoft.Heatmaps.Core;

public sealed class MapPoint(Coordinate coordinate, decimal value) : ValueObject<MapPoint>
{
    public decimal Value { get; } = value;

    public Coordinate Coordinate { get; } = coordinate;


    protected override bool EqualsCore(MapPoint other) =>
        other.Value == this.Value && other.Coordinate == this.Coordinate;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(this.Value, this.Coordinate);
}

public static class MapPointFunctions
{
    public static MapPoint CalculateCenterPoint(this IReadOnlyCollection<MapPoint> points)
    {
        if (points.Count == 0)
        {
            throw new ArgumentException($"{nameof(points)} must contain at least one element");
        }

        decimal value = points.Sum(p => p.Value) / points.Count;
        Coordinate center = Bounds.FromCoordinates(points.Select(p => p.Coordinate)).Center;
        return new MapPoint(center, value);
    }
}