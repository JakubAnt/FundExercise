using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exercise.UI.Extenxions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> NullToEmpty<T>(this IEnumerable<T> enumeration)
        {
            return enumeration ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<T> SafeCast<T>(this IList toCast)
        {
            return toCast?.Cast<T>() ?? Enumerable.Empty<T>();
        }
    }
}
