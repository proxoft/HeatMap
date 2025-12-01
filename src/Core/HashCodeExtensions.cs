namespace Proxoft.Heatmaps.Core;

internal static class HashCodeExtensions
{
    public static int AggregatedGetHashCode<T>(this IEnumerable<T?> source)
    {
        HashCode hashCode = new();
        foreach (T? i in source)
        {
            hashCode.Add(i);
        }

        return hashCode.ToHashCode();
    }
}
