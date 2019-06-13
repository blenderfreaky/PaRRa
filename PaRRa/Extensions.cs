using System;
using System.Collections.Generic;
using System.Text;

namespace PaRRa
{
    internal static class Extensions
    {
        internal static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        internal static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            if (!dict.TryGetValue(key, out TValue val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }

        internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T elem in enumerable)
            {
                action(elem);
            }
        }

        internal static bool AddIfNew<T>(this ICollection<T> collection, T elem)
        {
            if (!collection.Contains(elem))
            {
                collection.Add(elem);
                return true;
            }

            return false;
        }
    }
}
