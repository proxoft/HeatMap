namespace Proxoft.Heatmaps.Core;

internal static class IsoBandFunctions
{
    extension(IsoBand isoBand)
    {
        public bool CanMerge(IsoBand other) =>
            isoBand.IsoPolygons.Any(p => other.IsoPolygons.Any(p2 => p2.CanMerge(p)));

        public IsoBand Merge(IsoBand second)
        {
            IsoPolygon[] polygons = [.. isoBand.IsoPolygons.TryMerge(second.IsoPolygons)];

            return polygons.Length == 0
                ? isoBand
                : new IsoBand(isoBand.Value, polygons);
        }
    }

    extension(IEnumerable<IsoBand> isoBands)
    {
        private IEnumerable<IsoBand> MergeNeighbours()
        {
            List<IsoBand> temp = [.. isoBands];

            IsoBand? current = null;
            List<IsoBand> merged = [];

            while (temp.Count > 0)
            {
                current ??= temp.PopFirst();
                IsoBand[] toMerge = [.. temp.Pop(p => p.CanMerge(current))];
                if (toMerge.Length == 0)
                {
                    merged.Add(current);
                    current = null;
                }
                else
                {
                    current = toMerge.Aggregate(
                        seed: current,
                        func: (acc, toMergeIsoBand) => acc.Merge(toMergeIsoBand)
                    );
                }
            }

            if (current is not null)
            {
                merged.Add(current);
            }

            return merged;
        }

        public IEnumerable<IsoBand> MergeNeighboursWithSameLevel() =>
            isoBands
                .GroupBy(i => i.Value)
                .SelectMany(g => g.MergeNeighbours());
    }
}
