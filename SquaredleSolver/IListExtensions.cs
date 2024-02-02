using System;
using System.Collections.Generic;

namespace SquaredleSolver;
public static class IListExtensions
{

    public static void OrderedInsert<T>(this IList<T> list, T value)
        where T : IComparable<T>
    {
        OrderedInsert(list, value, Comparer<T>.Default);
    }

    public static void OrderedInsert<T>(this IList<T> list, T value, Comparer<T> comparer)
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (comparer.Compare(list[i], value) >= 0)
            {
                list.Insert(i, value);
                return;
            }
        }

        list.Add(value);
    }
}
