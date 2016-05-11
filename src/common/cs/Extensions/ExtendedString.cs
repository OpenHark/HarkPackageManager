using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace System
{
    public static class ExtendedString
    {
        public static byte[] GetBytes(this string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }
        
        public static Regex ToWildcardRegex(this string regex)
        {
            return new Regex(regex.Replace("*", ".*"));
        }
    }   
}