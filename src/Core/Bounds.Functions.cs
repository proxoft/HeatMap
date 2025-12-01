namespace Proxoft.Heatmaps.Core;

internal static class BoundsFunctions
{
    extension (Bounds bounds)
    {
        public Bounds Merge(Bounds other) =>
            new(
                Left: Math.Min(bounds.Left, other.Left),
                Top: Math.Max(bounds.Top, other.Top),
                Right: Math.Max(bounds.Right, other.Right),
                Bottom: Math.Min(bounds.Bottom, other.Bottom)
            );
    }
}
