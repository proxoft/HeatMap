namespace Proxoft.Heatmaps.Core.Internals;

internal static class LinearInterpolation
{
    public enum CoefficientRounding
    {
        Ceil,
        Round,
        Floor
    }

    public static decimal Coeficient(decimal ofValue, decimal from, decimal to)
    {
        if (from == to && from == ofValue)
        {
            return 0;
        }

        if (from <= to && (ofValue < from || ofValue > to))
        {
            throw new ArgumentOutOfRangeException($"value {ofValue} must be between {from} and {to}");
        }

        if (from >= to && (ofValue > from || ofValue < to))
        {
            throw new ArgumentOutOfRangeException($"value {ofValue} must be between {from} and {to}");
        }

        return (ofValue - from) / (to - from);
    }

    public static decimal CoeficientSafe(decimal ofValue, decimal from, decimal to)
    {
        if(ofValue < from)
        {
            return 0;
        }

        if(ofValue > to)
        {
            return 1;
        }

        return Coeficient(ofValue, from, to);
    }

    public static decimal InterpolateValue(decimal coeficient, decimal from, decimal to)
    {
        if(from == to)
        {
            return from;
        }

        return from + coeficient * (to - from);
    }

    public static int InterpolateValue(decimal coeficient, int from, int to, CoefficientRounding rounding = CoefficientRounding.Round)
    {
        if (from == to)
        {
            return from;
        }

        decimal interpolated = coeficient * (to - from);
        return from + interpolated.RoundTo(rounding);
    }

    public static int Interpolate(
        decimal value,
        (decimal, decimal) fromDomain,
        (int, int) toDomain,
        CoefficientRounding rounding = CoefficientRounding.Round)
    {
        decimal coefficient = Coeficient(value, fromDomain.Item1, fromDomain.Item2);
        decimal interpolated = InterpolateValue(coefficient, toDomain.Item1, toDomain.Item2);
        return interpolated.RoundTo(rounding);
    }

    private static int RoundTo(this decimal value, CoefficientRounding rounding)
    {
        return rounding switch
        {
            CoefficientRounding.Ceil => (int)System.Math.Ceiling(value),
            CoefficientRounding.Round => (int)System.Math.Round(value),
            CoefficientRounding.Floor => (int)System.Math.Floor(value),
            _ => throw new NotImplementedException($"rounding for {rounding} not supported")
        };
    }

    public static decimal Interpolate(
        decimal value,
        (decimal a, decimal b) fromRange,
        (decimal a, decimal b) inRange)
    {
        decimal k = Coeficient(value, fromRange.a, fromRange.b);
        return InterpolateValue(k, inRange.a, inRange.b);
    }
}