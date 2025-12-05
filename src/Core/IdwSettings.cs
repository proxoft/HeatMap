namespace Proxoft.Heatmaps.Core;

public class IdwSettings(
    int columnsCount = 10,
    int rowsCount = 10,
    int nClosestPoints = 4,
    int weighingPower = 2)
{
    public static readonly IdwSettings Default = new(10, 10, 4, 2);
    public int ColumnsCount { get; } = columnsCount;
    public int RowsCount { get; } = rowsCount;
    public int NClosestPoints { get; } = nClosestPoints;
    public int WeighingPower { get; } = weighingPower;
}
