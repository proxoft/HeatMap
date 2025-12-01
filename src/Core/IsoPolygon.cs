using Proxoft.Heatmaps.Core.Internals;

namespace Proxoft.Heatmaps.Core;

public sealed class IsoPolygon : ValueObject<IsoPolygon>
{
    private readonly Coordinate[] _points;
    private readonly Edge[] _edges;
    private int _hashCode = 0;

    public IsoPolygon(decimal value, Coordinate[] points)
    {
        if (points.Length < 3)
        {
            throw new ArgumentException("At least 3 points must be present");
        }

        this.Value = value;
        _points = points;
        _edges = [
            ..points
                .Select((p, i) => {
                    int nextPointIndex = (i + 1) % points.Length;
                    return new Edge(p, points.ElementAt(nextPointIndex));
                })
        ];
    }

    public decimal Value { get; }

    protected override bool EqualsCore(IsoPolygon other) =>
        this.Value == other.Value
        && _points.SequenceEqual(other._points);

    protected override int GetHashCodeCore()
    {
        if (_hashCode == 0)
        {
            _hashCode = HashCode.Combine(this.Value, _points.AggregatedGetHashCode());
        }

        return _hashCode;
    }
}
