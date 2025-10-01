using System.Diagnostics;

namespace AppBoot.SystemEx.Priority;

public static class PriorityExtensions
{
    /// <summary>
    ///     Orders the elements by priority attribute on their type.
    /// </summary>
    public static IEnumerable<T> OrderByPriority<T>(this IEnumerable<T> items)
    {
        return items.OrderBy(i => i, new PriorityComparer<T>());
    }

    private class PriorityComparer<T> : IComparer<T>
    {
        public int Compare(T? x, T? y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);

            PriorityAttribute? xAttribute = ReflectionExtensions.GetAttribute<PriorityAttribute>(x);
            PriorityAttribute? yAttribute = ReflectionExtensions.GetAttribute<PriorityAttribute>(y);

            int priorityX = xAttribute?.Value ?? int.MaxValue / 2;
            int priorityY = yAttribute?.Value ?? int.MaxValue / 2;

            int result = priorityX.CompareTo(priorityY);
            return result;
        }
    }
}