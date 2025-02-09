using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Siqqle;

internal static class EnumerableExtensions
{
    public static IReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyCollection<T>(source.ToList());
    }

    public static void IfAny<T>(this IEnumerable<T> source, Action<IEnumerable<T>> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (action == null)
            return;

        if (source.Any())
        {
            action(source);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (action == null)
            return;

        foreach (var item in source)
        {
            action(item);
        }
    }

    public static void For<T>(this IEnumerable<T> source, Action<int, T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (action == null)
            return;

        int index = 0;
        foreach (var item in source)
        {
            action(index++, item);
        }
    }
}
