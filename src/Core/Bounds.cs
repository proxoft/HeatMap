namespace Proxoft.Heatmaps.Core;

public record Bounds(decimal Left, decimal Top, decimal Right, decimal Bottom)
{
    public static readonly Bounds None = new(0, 0, 0, 0);

    public Coordinate Center => new(
            X: (this.Left + this.Right) / 2,
            Y: (this.Top + this.Bottom) / 2
        );

    public decimal Width => this.Right - this.Left;

    public decimal Height => this.Top - this.Bottom;

    public static Bounds FromCoordinates(IEnumerable<Coordinate> coordinates) =>
        FromCoordinates([..coordinates]);

    public static Bounds FromCoordinates(IReadOnlyCollection<Coordinate> coordinates)
    {
        if (coordinates.Count == 0) return None;

        decimal left = coordinates.Min(c => c.X);
        decimal right = coordinates.Max(c => c.X);
        decimal top = coordinates.Max(c => c.Y);
        decimal bottom = coordinates.Min(c => c.Y);

        return new Bounds(left, top, right, bottom);
    }
}