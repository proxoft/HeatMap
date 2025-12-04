namespace Proxoft.Heatmaps.Core.Exports;

internal static class IsoPolygonExport
{
    public static string ExportToPolygone(this IsoPolygon polygon) =>
        polygon.Points.Format();
}
