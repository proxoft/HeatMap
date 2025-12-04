namespace Proxoft.Heatmaps.Core;

internal sealed class IdwLine : ValueObject<IdwLine>
{
    private readonly MapPoint[] _points;
    private int _hashcode = 0;

    public IdwLine(params MapPoint[] points)
    {
        if (points.Length < 2)
        {
            throw new Exception("line must contain at least 2 points");
        }

        _points = points;
    }

    public IEnumerable<MapPoint> Points => _points;

    public IdwLine TrySplit(params decimal[] levels)
    {
        IdwLine split = levels.Aggregate(
            seed: this,
            func: (acc, l) => acc.TrySplitByValue(l));

        return split;
    }

    public static IdwLine From(MapPoint a, MapPoint b, decimal[] splitLevels)
    {
        IdwLine l = new IdwLine(a, b)
            .TrySplit(splitLevels);

        return l;
    }

    protected override bool EqualsCore(IdwLine other) =>
        other.GetHashCode() == this.GetHashCode()
            && other._points.SequenceEqual(_points);

    protected override int GetHashCodeCore()
    {
        if (_hashcode == 0)
        {
            _hashcode = _points.AggregatedGetHashCode();
        }

        return _hashcode;
    }

    private IdwLine TrySplitByValue(decimal value)
    {
        if (!value.IsValueBetween(_points.First(), _points.Last()))
        {
            return this;
        }

        (MapPoint a, MapPoint b) = this.FindSplitPoints(value);
        if (a.Value == value || b.Value == value)
        {
            return this;
        }

        decimal k = LinearInterpolation.Coeficient(value, a.Value, b.Value);
        decimal x = LinearInterpolation.InterpolateValue(k, a.Coordinate.X, b.Coordinate.X);
        decimal y = LinearInterpolation.InterpolateValue(k, a.Coordinate.Y, b.Coordinate.Y);

        MapPoint mp = new(new Coordinate(x, y), value);

        int i = Array.IndexOf(_points, a);
        MapPoint[] p = [
            .._points.Take(i + 1),
            mp,
            .._points.Skip(i + 1)
        ];

        return new IdwLine(p);
    }

    private (MapPoint, MapPoint) FindSplitPoints(decimal value)
    {
        for (int i = 0; i < _points.Length - 1; i++)
        {
            // todo: should be strictly between. If value is equal to one of the points, no split is possible
            if (value.IsValueBetween(_points[i], _points[i + 1]))
            {
                return (_points[i], _points[i + 1]);
            }
        }

        throw new Exception("cannot find split points");
    }
}

file static class Functions
{
    public static bool IsValueBetween(this decimal value, MapPoint p1, MapPoint p2) =>
        value.IsBetween(p1.Value, p2.Value);

    public static bool IsBetween(this decimal value, decimal left, decimal right) =>
        left <= right && value >= left && value <= right
        || left > right && value <= left && value >= right;
}