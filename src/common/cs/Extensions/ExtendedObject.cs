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
    public static class ExtendedObject
    {
        public static IEnumerable<TSource> ToPipe<TSource>(this TSource source)
        {
            if(source == null)
                return new TSource[0];
            else
                return new TSource[] { source };
        }
    }
}