namespace Proxoft.Heatmaps.Core.Internals;

internal static class ListExtensions
{
    extension<T>(List<T> list)
    {
        public void AddLeft(T item) =>
            list.Insert(0, item);

        public void AddRight(T item) =>
            list.Add(item);
    }
}
