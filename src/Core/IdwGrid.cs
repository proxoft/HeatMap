namespace Proxoft.Heatmaps.Core;

public class IdwGrid(GridNode[] nodes)
{
    public static readonly IdwGrid None = new([]);

    private readonly GridNode[] _nodes = nodes;

    private readonly int _colCount = nodes
            .Select(n => n.GridCoordinate.Column)
            .DefaultIfEmpty(0)
            .Max();

    private readonly int _rowCount = nodes
            .Select(n => n.GridCoordinate.Row)
            .DefaultIfEmpty(0)
            .Max();

    public IEnumerable<GridNode> Nodes => _nodes;

    public int ColCount => _colCount;

    public int RowCount => _rowCount;
}

internal static class IdwGridExtensions
{
    extension(IdwGrid grid)
    {
        public IEnumerable<IdwCell> Cells => Enumerable
            .Range(0, Math.Max(0, grid.RowCount - 1))
            .SelectMany(rowindex =>
                Enumerable
                    .Range(0, Math.Max(0, grid.ColCount - 1))
                    .Select(colindex => grid.Cell(rowindex, colindex))
            );

        private IdwCell Cell(int rowIndex, int colIndex)
        {
            GridNode nw = grid.Find(rowIndex, colIndex);
            GridNode ne = grid.Find(rowIndex, colIndex + 1);
            GridNode se = grid.Find(rowIndex + 1, colIndex + 1);
            GridNode sw = grid.Find(rowIndex + 1, colIndex);

            return new IdwCell(new GridCoordinate(colIndex, rowIndex), nw, ne, se, sw);
        }

        private GridNode Find(int rowIndex, int colIndex) =>
            grid.Nodes
                .Single(gp => gp.GridCoordinate.Row == rowIndex && gp.GridCoordinate.Column == colIndex);
    }
}