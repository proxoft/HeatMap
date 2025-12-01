namespace Proxoft.Heatmaps.Core;

internal static class CoordinateFunctions
{
    extension(Coordinate coordinate)
    {
        public decimal DistanceTo(Coordinate other)
        {
            if (coordinate == other) return 0;

            decimal dx = coordinate.X - other.X;
            decimal dy = coordinate.Y - other.Y;

            double d = Math.Sqrt((double)(dx * dx + dy * dy));
            return (decimal)d;
        }
    }
}
