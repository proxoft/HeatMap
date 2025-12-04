namespace Proxoft.Heatmaps.Core;

public class IdwSettings(
    int columnsCount,
    int rowsCount,
    int nClosestPoints,
    int weighingPower)
{
    public static readonly IdwSettings Default = new(10, 10, 4, 2);
    public int ColumnsCount { get; } = columnsCount;
    public int RowsCount { get; } = rowsCount;
    public int NClosestPoints { get; } = nClosestPoints;
    public int WeighingPower { get; } = weighingPower;
}
