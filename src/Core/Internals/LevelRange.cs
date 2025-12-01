namespace Daves.Heatmaps.Core.Internals;

internal record struct LevelRange(decimal From, decimal To)
{
    public static readonly LevelRange None = new(0, 0);

    public readonly bool IsInRange(decimal value) =>
        this.From <= value && value <= this.To;

    public static IEnumerable<LevelRange> FromLevels(ICollection<decimal> levels) =>
        levels
            .SkipLast(1)
            .Select((l, i) => new LevelRange(l, levels.ElementAt(i + 1)));
}
