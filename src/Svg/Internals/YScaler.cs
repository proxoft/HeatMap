namespace Proxoft.Heatmaps.Svg.Internals;

internal class YScaler(decimal top)
{
    private readonly decimal _top = top;

    public decimal ToSvgY(decimal y) =>
        _top - y;
}
