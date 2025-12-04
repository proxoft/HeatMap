namespace Proxoft.Heatmaps.Core;

internal static class IdwGridFunctions
{
    extension(IdwGrid grid)
    {
        public (IsoLine[], IsoBand[]) CalculateIsoLinesAndIsoBands(decimal[] levels)
        {
            IdwTriangle[] triangles = [
            ..grid.Cells.SelectMany(c => c.SplitToTriangles(levels))
            ];

            IsoLine[] isoLines = triangles.CreateIsoLines();
            IsoBand[] isoBands = triangles.CreateIsoBands(levels);

            return (isoLines, isoBands);
        }

        public decimal[] CalculateLevels()
        {
            if (grid == IdwGrid.None) return [];

            decimal min = grid.Nodes.Select(n => n.MapPoint.Value).Min();
            decimal max = grid.Nodes.Select(n => n.MapPoint.Value).Max();

            int levelCount = 6;
            decimal range = max - min;
            decimal step = range / (levelCount - 1);

            decimal[] levels = [
                .. Enumerable
                    .Range(0, levelCount)
                    .Select(i => i == levelCount - 1
                        ? max
                        : min + step * i
                )
                // .Select(s => Math.Round(s))
            ];

            return levels;
        }
    }
}

internal static class IdwGridCalculation
{
    public static IdwGrid CalculateIdw(
        this IReadOnlyCollection<Item> items,
        Bounds bounds,
        IdwSettings settings)
    {
        GridNode[] idwPoints = [
            ..Enumerable.Range(0, settings.RowsCount + 1)
                .SelectMany(i => CalculateRow(i, items, bounds, settings))
        ];

        return new IdwGrid(idwPoints);
    }

    private static GridNode[] CalculateRow(
        int rowIndex,
        IReadOnlyCollection<Item> records,
        Bounds bounds,
        IdwSettings settings)
    {
        decimal y = CalculateRowY(bounds, rowIndex, settings.RowsCount);
        GridNode[] rowPoints = [.. Enumerable.Range(0, settings.ColumnsCount + 1)
            .Select(columnIndex =>
            {
                decimal x = CalculateColX(bounds, columnIndex, settings.ColumnsCount);
                Coordinate coordinate = new(x, y);
                GridCoordinate gridCoordinate = new(columnIndex, rowIndex);
                return records.CalculatePoint(coordinate, gridCoordinate, settings);
            })];

        return rowPoints;
    }

    private static decimal CalculateColX(Bounds bounds, int colIndex, int colCount)
    {
        if (colIndex == 0) return bounds.Left;
        if (colIndex == colCount) return bounds.Right;

        decimal lngDelta = Math.Abs(bounds.Right - bounds.Left) / colCount;
        return bounds.Left + lngDelta * colIndex;
    }

    private static decimal CalculateRowY(Bounds bounds, int rowIndex, int rowCount)
    {
        if (rowIndex == 0) return bounds.Top;
        if (rowIndex == rowCount) return bounds.Bottom;

        decimal latDelta = Math.Abs(bounds.Top - bounds.Bottom) / rowCount;
        return bounds.Top - latDelta * rowIndex;
    }

    private static GridNode CalculatePoint(
        this IEnumerable<Item> items,
        Coordinate position,
        GridCoordinate gridPosition,
        IdwSettings settings)
    {
        decimal value = 0;

        (Item item, decimal distance)[] closest = [..position.FindClosest(items, settings.NClosestPoints)];

        value = closest.Any(data => data.distance == 0)
            ? closest.CalculateGridPointValueAsAverage()
            : closest.CalculateGridPointValueAsWeightedAverage();

        MapPoint mapPoint = new(position, value);
        return new GridNode(gridPosition, mapPoint);
    }

    private static decimal CalculateGridPointValueAsWeightedAverage(this IEnumerable<(Item item, decimal distance)> distancedItems)
    {
        var result = distancedItems
                .Aggregate(
                    seed: new
                    {
                        weightedDistanceSum = 0m,
                        distanceSum = 0m
                    },
                    func: (acc, data) =>
                    {
                        decimal ds = 1 / data.distance;

                        return new
                        {
                            weightedDistanceSum = acc.weightedDistanceSum + data.item.Value * ds,
                            distanceSum = acc.distanceSum + ds,
                        };
                    })
        ;

        return result.weightedDistanceSum / result.distanceSum;
    }

    private static decimal CalculateGridPointValueAsAverage(this IEnumerable<(Item item, decimal distance)> distancedItems) =>
        distancedItems
            .Where(d => d.distance == 0)
            .Select(d => d.item.Value)
            .Average();

    private static IEnumerable<(Item item, decimal distance)> FindClosest(
        this Coordinate coordinate,
        IEnumerable<Item> items,
        int closesCount) =>
        items
           .Select(item => {
               decimal distance = item.Coordinate.DistanceTo(coordinate);
               return (item, distance);
           })
           .OrderBy(d => d.distance)
           .Take(closesCount);
}
