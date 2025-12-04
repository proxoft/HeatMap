namespace Proxoft.Heatmaps.Svg;

internal record Padding(decimal Left, decimal Top, decimal Right, decimal Bottom)
{
    public static readonly Padding Default = new(10, 10, 10, 10);
    public decimal Horizontal => this.Left + this.Right;
    public decimal Vertical => this.Top + this.Bottom;
}
