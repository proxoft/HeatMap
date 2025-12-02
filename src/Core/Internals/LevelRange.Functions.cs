namespace Proxoft.Heatmaps.Core.Internals;

internal static class LevelRangeFunctions
{
    public static LevelRange GetLevelRange(
        this IEnumerable<decimal> values,
        LevelRange[] ranges)
    {
        decimal maxValue = values.Max();
        return ranges.First(r => r.IsInRange(maxValue));
    }
}
