using System.Diagnostics;

namespace Proxoft.Heatmaps.Core.Internals;

[DebuggerDisplay("{this.DebugString()}")]
internal record class IdwTriangle(
    IdwLine a,
    IdwLine b,
    IdwLine c)
{
    public IdwLine A { get; } = a;
    public IdwLine B { get; } = b;
    public IdwLine C { get; } = c;

    private string DebugString() =>
        $"A: {this.A.Points.First()}; B: {this.B.Points.First()}; C: {this.C.Points.First()}";

    public static IdwTriangle FromPoints(MapPoint a, MapPoint b, MapPoint c, decimal[] splitEdgesLevels) =>
        new(
            IdwLine.From(a, b, splitEdgesLevels),
            IdwLine.From(b, c, splitEdgesLevels),
            IdwLine.From(c, a, splitEdgesLevels)
        );
}
