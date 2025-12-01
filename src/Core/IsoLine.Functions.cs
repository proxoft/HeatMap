namespace Proxoft.Heatmaps.Core;

internal static class IsoLineFunctions
{
    private enum CommonPoint
    {
        None,
        FirstFirst,
        LastFirst,
        FirstLast,
        LastLast
    }

    extension(IsoLine isoLine)
    {
        public bool CanConcat(IsoLine other) =>
            isoLine.Value == other.Value
            && isoLine.CommonEdgePoint(other) != CommonPoint.None;

        public IsoLine Concat(IsoLine[] others, bool removeLastIfSameAsFirst)
        {
            IsoLine newLine = others
                .Aggregate(
                    seed: isoLine,
                    func: (acc, l) => acc.Concat(l, removeLastIfSameAsFirst)
                );

            return newLine;
        }

        private IsoLine Concat(IsoLine other, bool removeLastIfSameAsFirst)
        {
            if (isoLine.Value != other.Value)
            {
                throw new ArgumentException($"cannot merge isolines with different values: {isoLine.Value} and {other.Value}");
            }

            CommonPoint cp = isoLine.CommonEdgePoint(other);
            if (cp == CommonPoint.None)
            {
                throw new ArgumentException("cannot merge isolines which don't have common starting point");
            }

            Coordinate[] points = cp switch
            {
                CommonPoint.FirstFirst => [.. isoLine.Points.Reverse(), .. other.Points.Skip(1)],
                CommonPoint.LastFirst => [.. isoLine.Points, .. other.Points.Skip(1)],
                CommonPoint.FirstLast => [.. isoLine.Points.Reverse(), .. other.Points.Reverse().Skip(1)],
                CommonPoint.LastLast => [.. isoLine.Points, .. other.Points.Reverse().Skip(1)],
                _ => throw new Exception("Unhandled common point case"),
            };

            if (removeLastIfSameAsFirst)
            {
                points = [.. points.RemoveLastIfSameAsFirst()];
            }

            return new IsoLine(points, isoLine.Value);
        }

        private CommonPoint CommonEdgePoint(IsoLine otherLine)
        {
            if (isoLine.Points.First() == otherLine.Points.First())
            {
                return CommonPoint.FirstFirst;
            }

            if (isoLine.Points.First() == otherLine.Points.Last())
            {
                return CommonPoint.FirstLast;
            }

            if (isoLine.Points.Last() == otherLine.Points.First())
            {
                return CommonPoint.LastFirst;
            }

            if (isoLine.Points.Last() == otherLine.Points.Last())
            {
                return CommonPoint.LastLast;
            }

            return CommonPoint.None;
        }
    }

    public static IsoLine[] Defragment(this IEnumerable<IsoLine> lines)
    {
        List<IsoLine> fragmented = [.. lines];

        if (fragmented.Count == 0)
        {
            return [];
        }

        List<IsoLine> defragmented = [
            fragmented[0]
        ];

        fragmented.RemoveAt(0);

        do
        {
            IsoLine current = defragmented.Last();
            IsoLine[] neighbours = [.. fragmented.Where(l => l.CanConcat(current))];

            if (neighbours.Length > 0)
            {
                IsoLine newCurrent = current.Concat(neighbours, removeLastIfSameAsFirst: false);
                defragmented.Remove(current);
                defragmented.Add(newCurrent);

                foreach (IsoLine l in neighbours)
                {
                    fragmented.Remove(l);
                }

                continue;
            }

            IsoLine futureCurrent = fragmented[0];
            fragmented.RemoveAt(0);
            defragmented.Add(futureCurrent);

        } while (fragmented.Count > 0);

        return [.. defragmented];
    }

    private static IReadOnlyCollection<T> RemoveLastIfSameAsFirst<T>(this IReadOnlyCollection<T> source)
        where T : IEquatable<T>
    {
        if (source.Count < 2) return source;
        return source.First().Equals(source.Last())
            ? [.. source.SkipLast(1)]
            : source;
    }
}
