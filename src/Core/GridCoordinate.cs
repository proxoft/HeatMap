namespace Proxoft.Heatmaps.Core;

public class GridCoordinate(int column, int row) : ValueObject<GridCoordinate>
{
    public int Column { get; } = column;

    public int Row { get; } = row;

    protected override bool EqualsCore(GridCoordinate other) =>
        other.Column == this.Column && other.Row == this.Row;

    protected override int GetHashCodeCore() =>
        HashCode.Combine(this.Column, this.Row);
}
