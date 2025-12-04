namespace Proxoft.Heatmaps.Core;

public record Coordinate(decimal X, decimal Y)
{
    public static implicit operator Coordinate((decimal x, decimal y) coordinates) =>
        new(coordinates.x, coordinates.y);
}

