namespace Proxoft.Heatmaps.Core;

public sealed class IsoBand : ValueObject<IsoBand>
{
    private int _hashCode = 0;

    public IsoBand(decimal value, IsoPolygon[] isoPolygons)
    {
        if (isoPolygons.Length == 0) throw new ArgumentException("There must be at least one polygon");

        this.Value = value;
        this.IsoPolygons = [.. isoPolygons.OrderByDescending(p => p.CalculateBounds().Area)];
    }

    public decimal Value { get; }

    public IsoPolygon OuterPolygon => this.IsoPolygons.ElementAt(0);

    // holes
    public IEnumerable<IsoPolygon> InnerPolygons => this.IsoPolygons.Skip(1);

    public IsoPolygon[] IsoPolygons { get; }

    protected override bool EqualsCore(IsoBand other) =>
        this.Value == other.Value
        && this.IsoPolygons.Length == other.IsoPolygons.Length
        && this.IsoPolygons.SequenceEqual(other.IsoPolygons);

    protected override int GetHashCodeCore()
    {
        if (_hashCode == 0)
        {
            _hashCode = HashCode.Combine(this.Value, this.IsoPolygons.AggregatedGetHashCode());
        }

        return _hashCode;
    }

    public static IsoBand From(IsoPolygon isoPolygon) =>
       new(isoPolygon.Value, [isoPolygon]);
}
