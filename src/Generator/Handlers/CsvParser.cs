using Proxoft.Heatmaps.Core;

namespace Proxoft.Heatmaps.Generator.Handlers;

internal static class CsvParser
{
    public static Either<GenerateError, Item[]> ReadItems(this string fileName, string separator, char decimalChar, int skipLines)
    {
        try
        {
            Item[] items = [
                ..File.ReadAllLines(fileName)
                    .Skip(skipLines)
                    .Select(line => line.ReadItem(separator, decimalChar))
            ];

            return items;
        }
        catch
        {
            return GenerateError.CsvParseError;
        }
        
    }

    private static Item ReadItem(this string line, string separator, char decimalChar) =>
        line.Split(separator) switch
        {
            [string x, string y, string value] => new Item((x.ToDecimal(decimalChar), y.ToDecimal(decimalChar)), value.ToDecimal(decimalChar)),
            _ => throw new Exception("Parse error")
        };

    private static decimal ToDecimal(this string value, char decimalSeparator) =>
        decimal.Parse(value.Replace(decimalSeparator, '.'), System.Globalization.CultureInfo.InvariantCulture);
}
