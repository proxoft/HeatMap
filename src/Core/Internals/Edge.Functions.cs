namespace Proxoft.Heatmaps.Core.Internals;

internal static class EdgeFunctions
{
    extension(Edge edge)
    {
        public bool HasPoint(Coordinate point) =>
            edge.A == point || edge.B == point;

        public bool AreEquivalent(Edge other) =>
            edge.HasPoint(other.A) && edge.HasPoint(other.B);

        public Edge Orientate(Coordinate start)
        {
            if(!edge.HasPoint(start)) throw new ArgumentOutOfRangeException(nameof(start), "starting point must be eiter A or B point of the edge");

            return edge.A == start
                ? edge
                : new(edge.B, edge.A);
        }
    }

    public static IEnumerable<IsoLine> ToDistinctIsoLines(this IReadOnlyCollection<Edge> edges)
    {
        if (edges.Count == 0) return [];

        IsoLine[] isoLines = [];
        List<Coordinate> isoLinePoints = [edges.First().A, edges.First().B];
        foreach (Edge e in edges.Skip(1))
        {
            if (isoLinePoints.HaveCommonPoint(e))
            {
                isoLinePoints.AppendEdge(e);
            }
            else
            {
                isoLines = isoLines.AddOrConcat(isoLinePoints.ToIsoLine());
                isoLinePoints = new() { e.A, e.B };
            }
        }

        if (isoLinePoints.Count > 0)
        {
            isoLines = isoLines.AddOrConcat(isoLinePoints.ToIsoLine());
        }

        return isoLines;
    }

    private static IsoLine ToIsoLine(this IEnumerable<Coordinate> points)
    {
        return new IsoLine([..points], 0);
    }

    private static bool HaveCommonPoint(this List<Coordinate> line, Edge e) =>
        e.A == line.First()
        || e.A == line.Last()
        || e.B == line.First()
        || e.B == line.Last();

    private static IsoLine[] AddOrConcat(this IReadOnlyCollection<IsoLine> isoLines, IsoLine newIsoLine)
    {
        IsoLine[] toMerge = isoLines.Where(i => i.CanConcat(newIsoLine)).ToArray();
        if (toMerge.Length == 0)
        {
            return isoLines.Append(newIsoLine).ToArray();
        }

        IsoLine merged = newIsoLine.Concat(toMerge, removeLastIfSameAsFirst: true);
        return isoLines
            .Except(toMerge)
            .Concat(new[] { merged })
            .ToArray();
    }

    private static void AppendEdge(this List<Coordinate> coordinates, Edge e)
    {
        if (coordinates.First() == e.A)
        {
            coordinates.Insert(0, e.B);
            return;
        }

        if (coordinates.First() == e.B)
        {
            coordinates.Insert(0, e.A);
            return;
        }

        if (coordinates.Last() == e.A)
        {
            coordinates.Add(e.B);
            return;
        }

        coordinates.Add(e.A);
    }
}
