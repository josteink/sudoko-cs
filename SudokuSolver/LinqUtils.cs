using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SudokuSolver
{
    public static class LinqUtils
    {
        public static IEnumerable<TItem> ExcludeOn<TItem, TIntersect>(this IEnumerable<TItem> list1,
            IEnumerable<TItem> list2, Func<TItem, TIntersect> valueGetter)
            where TIntersect : IEquatable<TIntersect>
        {
            // keep only items from list1 when value from valueGuetter does NOT collide with list2
            var avoidValues = list2.Select(valueGetter);

            var result = 
                from i in list1
                let v = valueGetter(i)
                where v.NotIn(avoidValues)
                select i;

            return result;
        }

        public static bool In<T>(this T item, IEnumerable<T> coll)
        {
            return coll.Contains(item);
        }

        public static bool NotIn<T>(this T item, IEnumerable<T> coll)
        {
            return !In(item, coll);
        }
    }
}
