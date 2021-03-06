using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace System.Linq
{
    public static class ExtendedLinq
    {
        public static IEnumerable<string> ToStrings<TSource>(this IEnumerable<TSource> source)
        {
            return source.Select(x => Convert.ToString(x));
        }
        
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach(TSource x in source)
                action(x);
        }
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            int i = 0;
            foreach(TSource x in source)
            {
                action(x, i);
                ++i;
            }
        }
        
        public static IEnumerable<TSource> Global<TSource>(this IEnumerable<TSource> source, Action<IEnumerable<TSource>> action)
        {
            action(source);
            return source;
        }
        
        public static IEnumerable<TSource> Peek<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            return source.Select(x => { action(x); return x; });
        }
        public static IEnumerable<TSource> Peek<TSource>(this IEnumerable<TSource> source, Action<TSource, int> action)
        {
            return source.Select((x,i) => { action(x, i); return x; });
        }
        
        public static void Close<TSource>(this IEnumerable<TSource> source)
        {
            foreach(TSource x in source);
        }
    }   
}