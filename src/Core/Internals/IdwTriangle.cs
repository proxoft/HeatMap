namespace Proxoft.Heatmaps.Core.Internals;

internal sealed class IdwTriangle(
    IdwLine a,
    IdwLine b,
    IdwLine c) : ValueObject<IdwTriangle>
{
    public IdwLine A { get; } = a;
    public IdwLine B { get; } = b;
    public IdwLine C { get; } = c;

    public static IdwTriangle FromPoints(MapPoint a, MapPoint b, MapPoint c, decimal[] splitEdgesLevels) =>
        new(
            IdwLine.From(a, b, splitEdgesLevels),
            IdwLine.From(b, c, splitEdgesLevels),
            IdwLine.From(c, a, splitEdgesLevels)
        );

    protected override bool EqualsCore(IdwTriangle other) =>
        other.A == this.A
        && other.B == this.B
        && other.C == this.C;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(this.A, this.B, this.C);
}
