namespace Proxoft.Heatmaps.Core.Internals;

internal static class IndexExtensions
{
    public static bool IsSameOrNeighbour(this Index left, Index right, int inArrayCount)
    {
        int li = left.AbsoluteIndex(inArrayCount);
        int ri = right.AbsoluteIndex(inArrayCount);
        return Math.Abs(li - ri) <= 1;
    }

    public static Index Next(this Index index)
    {
        return new Index(index.Value + 1, fromEnd: index.IsFromEnd);
    }

    private static int AbsoluteIndex(this Index index, int inArrayCount) =>
        index.IsFromEnd
            ? inArrayCount - index.Value
            : index.Value;
}
