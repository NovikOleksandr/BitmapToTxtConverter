using System.Collections.Generic;

namespace Signals.Extensions
{
    public static class CollectionsExtensions
    {

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                list.Add(element);
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue>[] items)
        {
            foreach (var item in items)
            {
                dictionary.Add(item);
            }
        }
    }
}
