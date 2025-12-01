namespace Proxoft.Heatmaps.Core.Internals;

internal sealed class Edge(Coordinate a, Coordinate b) : ValueObject<Edge>
{
    private readonly Coordinate _a = a;
    private readonly Coordinate _b = b;

    protected override bool EqualsCore(Edge other) =>
        _a == other._a && _b == other._b;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(_a, _b);
}
