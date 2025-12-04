namespace Proxoft.Heatmaps.Core;

public sealed class IsoLine(Coordinate[] points, decimal value) : ValueObject<IsoLine>
{
    private int _hashCode = 0;

    public Coordinate[] Points { get; } = points;

    public decimal Value { get; } = value;

    public bool CanConnectTo(IsoLine other) =>
        this.Value == other.Value
        && (this.Points.First() == other.Points.First()
            || this.Points.First() == other.Points.Last()
            || this.Points.Last() == other.Points.First()
            || this.Points.Last() == other.Points.Last());

    protected override bool EqualsCore(IsoLine other) =>
        other.Value == this.Value
        && other.Points.Length == this.Points.Length
        && other.Points.SequenceEqual(this.Points);

    protected override int GetHashCodeCore()
    {
        if(_hashCode == 0)
        {
            _hashCode = HashCode.Combine(
                this.Value,
                this.Points.AggregatedGetHashCode()
            );
        }

        return _hashCode;
    }
}
