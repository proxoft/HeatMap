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

    extension (IReadOnlyCollection<Coordinate> coordinates)
    {
        public IEnumerable<Coordinate> Clockwise()
        {
            if (coordinates.Count < 3)
            {
                return coordinates;
            }

            bool isClockwise = coordinates.IsClockwise();
            return isClockwise
                ? coordinates
                : coordinates.Reverse();
        }

        public IEnumerable<Coordinate> CounterClockwise()
        {
            if (coordinates.Count < 3)
            {
                return coordinates;
            }

            bool isClockwise = coordinates.IsClockwise();
            return isClockwise
                ? coordinates.Reverse()
                : coordinates;
        }

        private bool IsClockwise()
        {
            decimal sum = 0.0m;
            Coordinate v1 = coordinates.ElementAt(^1); // last element
            for (int i = 0; i < coordinates.Count; i++)
            {
                Coordinate v2 = coordinates.ElementAt(i);
                sum += (v2.X + v1.X) * (v2.Y - v1.Y);
                v1 = v2;
            }

            return sum > 0.0m;
        }
    }
}
