namespace Proxoft.Heatmaps.Generator.Commands;

[Verb("g", HelpText = "generate heatmap")]
internal class GenerateCommand
{
    [Option(shortName: 'c', "csv", Required = true, HelpText = "Path to csv source file. Expected format x y value.")]
    public string? CsvSourceFile { get; set; }

    [Option('s', "separator", Default = "\t", HelpText = "Value separator, default tab")]
    public string? CsvSeparator { get; set; }

    [Option('d', "decimal", Default = '.', HelpText = "Decimal seperator in numbers (default '.')")]
    public char CsvDecimalChar { get; set; } = '.';

    [Option('s', "skiplines", Default = 1, HelpText = "Skip first n lines, e.g. if it contains captions")]
    public int CsvSkipFirstLine { get; set; }

    [Option('o', "output", Required = true, HelpText = "Output file path without extension, e.g. C:/Temp/exampleHeatMap")]
    public string? OutputFile { get; set; }

    [Option('t', "continue", Default = false, HelpText = "Continue working (Do not close console)")]
    public bool Continue { get; set; }
}
