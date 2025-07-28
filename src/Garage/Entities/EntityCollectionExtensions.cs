namespace Garage.Entities;

public static class EntityCollectionExtensions
{
    public static List<T> ToSortedList<T>(this IEnumerable<T> source) where T : ISortable
    {
        return source.OrderBy(x => x.SortIndex).ToList();
    }
}