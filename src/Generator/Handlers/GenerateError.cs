namespace Proxoft.Heatmaps.Generator.Handlers;

internal enum GenerateError
{
    NotImplemented,
    CsvParseError,
    ErrorCalculatingHeatmap,
    IdwGridExportFailed,
    SvgExportFailed
}
