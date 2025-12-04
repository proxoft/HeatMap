namespace Proxoft.Heatmaps.Core.Internals;

internal record IdwCell(
    GridCoordinate Position,
    GridNode TopLeft,
    GridNode TopRight,
    GridNode BottomRight,
    GridNode BottomLeft
);
