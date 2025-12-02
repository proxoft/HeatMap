namespace Proxoft.Heatmaps.Core;

internal static class IsoPolygonFunctions
{
    public static Bounds CalculateBounds(this IsoPolygon polygon) =>
        Bounds.FromCoordinates(polygon.Points);
}
