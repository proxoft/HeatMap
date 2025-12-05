using Proxoft.Heatmaps.Core;
using Proxoft.Heatmaps.Svg;

namespace Proxoft.Heatmaps.Generator.Handlers;

internal static class Export
{
    public static Option<GenerateError> ExportToFiles(this HeatMap heatMap, string filePathPattern)
    {
        try
        {
            heatMap.ExportIdwGrid(filePathPattern, "\t");
        }
        catch
        {
            return GenerateError.IdwGridExportFailed;
        }

        try
        {
            heatMap.ExportSvg(filePathPattern);
        }
        catch
        {
            return GenerateError.SvgExportFailed;
        }

        return None.Instance;
    }

    private static void ExportIdwGrid(this HeatMap heatMap, string filePathPattern, string separator)
    {
        string[] lines = [..heatMap.IdwGrid.Rows
            .Select(mapPoints => string.Join(separator, mapPoints.Select(p => $"{p.Value}")))
        ];

        File.WriteAllLines($"{filePathPattern}.txt", lines);
    }

    private static void ExportSvg(this HeatMap heatMap, string filePathPattern)
    {
        SvgOptions options = new()
        {
            IsoBands = true
        };
        string svg = heatMap.ToSvg(options);
        File.WriteAllText($"{filePathPattern}.svg", svg);
    }
}
