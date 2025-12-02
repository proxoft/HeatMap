namespace Proxoft.Heatmaps.Core.Tests.Common;

internal static class IdwGridFactory
{
    public static IdwGrid Create(
        decimal[][] values,
        decimal cellWidth = 10,
        decimal cellHeight = 10,
        decimal startX = 0,
        decimal startY = 0)
    {
        List<GridNode> nodes = [];
        for (int row = 0; row < values.Length; row++)
        {
            for (int column = 0; column < values[row].Length; column++)
            {
                decimal x = startX + cellWidth * column;
                decimal y = startY + cellHeight * (values.Length - row - 1);

                GridNode gridNode = new((column, row), new MapPoint((x, y), values[row][column]));
                nodes.Add(gridNode);
            }
        }

        return new IdwGrid([..nodes]);
    }
}
