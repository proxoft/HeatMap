namespace Proxoft.Heatmaps.Core;

internal static class BoundsFunctions
{
    extension (Bounds bounds)
    {
        public bool Intersects(Bounds other)
        {
            bool distinct= bounds.Right < other.Left
                || bounds.Left > other.Right
                || bounds.Bottom > other.Top
                || bounds.Top < other.Bottom;

            return !distinct;
        }

        public Bounds Merge(Bounds other) =>
            new(
                Left: Math.Min(bounds.Left, other.Left),
                Top: Math.Max(bounds.Top, other.Top),
                Right: Math.Max(bounds.Right, other.Right),
                Bottom: Math.Min(bounds.Bottom, other.Bottom)
            );

        public decimal Area => bounds.Width * bounds.Height;
    }
}
