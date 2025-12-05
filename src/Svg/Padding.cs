namespace Proxoft.Heatmaps.Svg;

internal record Padding(decimal Left, decimal Top, decimal Right, decimal Bottom)
{
    public static readonly Padding Zero = new(0, 0, 0, 0);

    public static readonly Padding Default = new(10, 10, 10, 10);

    public decimal Horizontal => this.Left + this.Right;

     public decimal Vertical => this.Top + this.Bottom;

    public static Padding All(decimal padding) =>
        new(padding, padding, padding, padding);
}
