namespace Proxoft.Heatmaps.Core.Internals;

internal static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public IReadOnlyCollection<T> Pop(Func<T, bool> predicate)
        {
            T[] popped = [.. list.Where(predicate)];
            list.RemoveAll(i => predicate(i));
            return popped;
        }

        public T PopFirst()
        {
            T i = list[0];
            list.RemoveAt(0);
            return i;
        }

        public T PopFirst(Func<T, bool> predicate)
        {
            T popped = list
                .Where(predicate)
                .First();

            list.Remove(popped);
            return popped;
        }

        public IEnumerable<T> PopFirstOrNone(Func<T, bool> predicate)
        {
            T[] popped = [
                ..list
                    .Where(predicate)
                    .Take(1)
            ];

            foreach (T p in popped)
            {
                list.Remove(p);
            }

            return popped;
        }

        public void AddLeft(T item) =>
            list.Insert(0, item);

        public void AddRight(T item) =>
            list.Add(item);
    }
}
