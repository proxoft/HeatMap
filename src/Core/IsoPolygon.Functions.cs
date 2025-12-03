using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Proxoft.Heatmaps.Core;

internal static class IsoPolygonFunctions
{
    extension(IsoPolygon polygon)
    {
        public Bounds CalculateBounds() =>
            Bounds.FromCoordinates(polygon.Points);

        public bool CanMerge(IsoPolygon other)
        {
            if (polygon.Value != other.Value)
            {
                return false;
            }

            if (!polygon.CalculateBounds().Intersects(other.CalculateBounds()))
            {
                return false;
            }

            bool anyCommonEdge = polygon.Edges.Any(e => other.Edges.Any(e2 => e2.AreEquivalent(e)));
            return anyCommonEdge;
        }

        public IEnumerable<IsoPolygon> Merge(IsoPolygon other)
        {
            (Edge e1, Edge e2)[] toRemovePairs = [
                ..polygon.Edges.Join(
                    other.Edges,
                    e => e,
                    e => e,
                    (e1, e2) => (e1, e2),
                    NonDirectionEdgeComparer.Instance)
            ];

            List<Edge> pRemaining = [.. polygon.Edges.Except(toRemovePairs.Select(r => r.e1))];
            List<Edge> otherRemaining = [.. other.Edges.Except(toRemovePairs.Select(r => r.e2))];

            return Merge(polygon.Value, pRemaining, otherRemaining);
        }

        public IEnumerable<IsoPolygon> SplitIfIsSelfCuttingPolygon()
        {
            return polygon.IsSelfCutting()
                ? polygon.SplitSelfCuttingPolygon()
                : [polygon];
        }

        private bool IsSelfCutting() =>
            polygon.Points.GroupBy(p => p).Any(g => g.Count() > 1);

        private IsoPolygon[] SplitSelfCuttingPolygon()
        {
            // find the self cutting point
            // start from the point by any edge and concat edges until polygon is closed
            // repeat until there's no other point

            List<IsoPolygon> polygons = [];
            List<Edge> edges = [.. polygon.Edges];
            while (edges.Count > 0)
            {
                Coordinate startingPoint = edges
                    .Select(e => e.A)
                    .GroupBy(p => p)
                    .Select(g => (point: g.First(), count: g.Count()))
                    .OrderByDescending(g => g.count)
                    .Select(g => g.point)
                    .First();

                Coordinate[] polygonPoints = edges.ExtractPolygon(startingPoint);
                if (polygonPoints.Length > 0)
                {
                    IsoPolygon p = new(polygon.Value, polygonPoints);
                    polygons.Add(p);
                }
            }

            return [.. polygons];
        }
    }

    extension(IEnumerable<IsoPolygon> polygons)
    {
        public IEnumerable<IsoPolygon> TryMerge(
            IReadOnlyCollection<IsoPolygon> otherPolygons)
        {
            List<IsoPolygon> temp = [..polygons, ..otherPolygons];

            IsoPolygon? current = null;
            List<IsoPolygon> merged = [];

            while (temp.Count > 0)
            {
                current ??= temp.PopFirst();

                IsoPolygon[] toMerge = [.. temp.PopFirstOrNone(p => p.CanMerge(current))];

                if (toMerge.Length == 0)
                {
                    merged.Add(current);
                    current = null;
                }
                else
                {
                    IsoPolygon[] mergeResult = [.. current.Merge(toMerge[0])];
                    current = mergeResult.FirstOrDefault();
                    temp.AddRange(mergeResult.Skip(1));
                }
            }

            if (current is not null)
            {
                merged.Add(current);
            }

            return merged;
        }
    }

    private static IsoPolygon[] Merge(decimal value, IReadOnlyCollection<Edge> p, IReadOnlyCollection<Edge> q)
    {
        List<IsoLine> merged = [];
        List<IsoLine> temp = [
            ..p.ToDistinctIsoLines(),
                ..q.ToDistinctIsoLines()
        ];

        IsoLine? current = null;
        while (temp.Count > 0)
        {
            current ??= temp.PopFirst();

            IsoLine[] toMerge = temp
                .Pop(l => l.CanConcat(current))
                .ToArray();

            if (toMerge.Length > 0)
            {
                current = current.Concat(toMerge, removeLastIfSameAsFirst: true);
            }
            else
            {
                merged.Add(current);
                current = null;
            }
        }

        if (current != null)
        {
            merged.Add(current);
        }

        IsoPolygon[] polygons = merged
            .Select(l => new IsoPolygon(value, l.Points.ToArray()))
            .SelectMany(p => p.SplitIfIsSelfCuttingPolygon())
            .ToArray();

        //bool x = polygons
        //    .Any(p => p.IsSelfCutting());

        return polygons;
    }

    private static Coordinate[] ExtractPolygon(this List<Edge> edges, Coordinate startingPoint)
    {
        List<Coordinate> polygonPoints = [];

        Edge current = edges.PopNextEdge(startingPoint);

        while (current.B != startingPoint)
        {
            polygonPoints.Add(current.A);
            current = edges.PopNextEdge(current.B);

            if (current.B == startingPoint)
            {
                polygonPoints.Add(current.A);
            }
        }

        return polygonPoints.ToArray();
    }

    private static Edge PopNextEdge(this List<Edge> edges, Coordinate startingPoint)
    {
        return edges
            .PopFirst(e => e.HasPoint(startingPoint))
            .Orientate(startingPoint);
    }
}

file class NonDirectionEdgeComparer : IEqualityComparer<Edge>
{
    public static readonly NonDirectionEdgeComparer Instance = new();

    public bool Equals(Edge? x, Edge? y)
    {
        if (x is null == y is null)
        {
            return true;
        }

        if (x is null != y is null)
        {
            return false;
        }

        return x!.AreEquivalent(y!);
    }

    private static int CoordinateComparison(Coordinate a, Coordinate b)
    {
        if (a == b) return 0;

        decimal ld = a.Y - b.Y;
        if (ld != 0)
        {
            return ld < 0
                ? -1
                : 1;
        }

        return a.X - b.X < 0
            ? -1
            : 1;
    }

    public int GetHashCode([DisallowNull] Edge obj)
    {
        List<Coordinate> sorted = [obj.A, obj.B];
        sorted.Sort(CoordinateComparison);
        int hc = HashCode.Combine(sorted[0], sorted[1]);
        return hc;
    }
}
