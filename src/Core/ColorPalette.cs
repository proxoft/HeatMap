using System.Drawing;

namespace Proxoft.Heatmaps.Core;

public class ColorPalette(params Color[] colors)
{
    public static readonly ColorPalette BlueRed = new(
         ColorPaletteFactory.BlueRedColors()
    );

    public static readonly ColorPalette RedBlue = new(
         [..ColorPaletteFactory.BlueRedColors().Reverse()]
    );

    private readonly Color[] _colors = colors;

    public Color InterpolateColor(
        decimal value,
        (decimal minimum, decimal maximum) valueRange) =>
        this.FindColorFor(value, valueRange, interpolateColor: true);

    public Color FindColorFor(
        decimal value,
        (decimal minimum, decimal maximum) valueRange,
        bool interpolateColor = true)
    {
        if (value <= valueRange.minimum)
        {
            return _colors[0];
        }

        if (value >= valueRange.maximum)
        {
            return _colors[^1];
        }

        decimal[] levels = this.CalculateColorLevels(valueRange.minimum, valueRange.maximum);
        int index = value.IndexOfLevel(levels);
        if (index == _colors.Length - 1)
        {
            return _colors[index];
        }

        Color c1 = _colors[index];

        if (!interpolateColor)
        {
            return c1;
        }

        Color c2 = _colors[index + 1];
        Color color = value.Interpolate(levels[index], levels[index + 1], c1, c2);
        return color;
    }

    private decimal[] CalculateColorLevels(decimal minimum, decimal maximum)
    {
        decimal levelSize = (maximum - minimum) / (_colors.Length - 1);
        decimal[] levels = [.. Enumerable.Range(0, _colors.Length)
            .Select(l => l == _colors.Length - 1
                ? maximum
                : minimum + (l * levelSize))];

        return levels;
    }
}

public static class ColorExtensions
{
    public static string ToHex(this Color c)
    {
        return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}

file static class Operators
{
    public static int IndexOfLevel(this decimal value, decimal[] levels)
    {
        for (int i = 0; i < levels.Length - 1; i++)
        {
            if (levels[i] <= value && value < levels[i + 1])
            {
                return i;
            }
        }

        return levels.Length - 1;
    }

    public static Color Interpolate(this decimal value, decimal from, decimal to, Color fromColor, Color toColor)
    {
        decimal coefficient = LinearInterpolation.Coeficient(value, from, to);
        byte r = (byte)LinearInterpolation.InterpolateValue(coefficient, fromColor.R, toColor.R);
        byte g = (byte)LinearInterpolation.InterpolateValue(coefficient, fromColor.G, toColor.G);
        byte b = (byte)LinearInterpolation.InterpolateValue(coefficient, fromColor.B, toColor.B);

        return Color.FromArgb(r, g, b);
    }
}

file static class ColorPaletteFactory
{
    public static Color[] BlueRedColors() => [
        Color.FromArgb(0, 0, 255),   // Blue
        Color.FromArgb(75, 0, 255),  // Violet
        Color.FromArgb(128, 0, 255), // Purple
        Color.FromArgb(255, 0, 127), // Magenta
        Color.FromArgb(255, 69, 0),  // Orange-Red
        Color.FromArgb(255, 140, 0), // Dark-Orange
        Color.FromArgb(255, 204, 0), // Yellow
        Color.FromArgb(255, 215, 0), // Gold/Yellow
        Color.FromArgb(255, 0, 0)    // Red
    ];
}

