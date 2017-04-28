namespace Nett.LinqExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;

    internal static class EnumerableExtensions
    {
        public static Type GetElementType(this IEnumerable enumerable)
        {
            var et = enumerable.GetType();
            return et.GetTypeInfo().IsGenericType
                ? et.GetGenericArguments()[0]
                : (from object e in enumerable select e.GetType()).FirstOrDefault();
        }

        public static IEnumerable<T> Select<T>(this IEnumerable enumerable, Func<object, T> selector)
        {
            return from object e in enumerable select selector(e);
        }
    }
}