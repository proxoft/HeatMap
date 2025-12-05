using Proxoft.Heatmaps.Core;
using Proxoft.Heatmaps.Generator.Commands;

namespace Proxoft.Heatmaps.Generator.Handlers;

internal static class GenerateHeatmap
{
    public static Option<GenerateError> CreateHeatmap(this GenerateCommand command) =>
        command.CsvSourceFile.Clean()
            .ReadItems(command.CsvSeparator.Clean(), command.CsvDecimalChar, command.CsvSkipFirstLine)
            .Map<GenerateError, Item[], HeatMap>(items =>
            {
                try
                {
                    return items.CalculateHeatMap(new IdwSettings(columnsCount: 20, rowsCount: 20));
                }
                catch
                {
                    return GenerateError.ErrorCalculatingHeatmap;
                }
            })
            .Map(map => Export.ExportToFiles(map, command.OutputFilePattern.Clean()))
            .Reduce(e => e);

    private static string Clean(this string input) =>
        input.Trim('\"');
}
