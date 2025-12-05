namespace Proxoft.Heatmaps.Core;

public interface IIsoLevelCalculator
{
    decimal[] CalculateLevels(IdwGrid grid);
}

public sealed class DefaultIsoLevelCalculator : IIsoLevelCalculator
{
    public static readonly DefaultIsoLevelCalculator Instance = new();

    private readonly AtLeastNLevelsCalculator _calculator = new(6);

    public decimal[] CalculateLevels(IdwGrid grid) =>
        _calculator.CalculateLevels(grid);
}

public sealed class AtLeastNLevelsCalculator(
    int levelCount) : IIsoLevelCalculator
{
    private readonly int _levelCount = levelCount;

    public decimal[] CalculateLevels(IdwGrid grid)
    {
        if (grid == IdwGrid.None) return [];

        decimal min = grid.Min;
        decimal max = grid.Max;

        decimal range = max - min;
        decimal step = range / (_levelCount - 1);

        decimal[] levels = [
            .. Enumerable
                    .Range(0, _levelCount)
                    .Select(i => i == _levelCount - 1
                        ? max
                        : min + step * i
                    )
                    .Select((l, i) => i == 0 ? Math.Floor(l) : i == _levelCount - 1 ? Math.Ceiling(l) : Math.Round(l))
        ];

        return levels;
    }
}