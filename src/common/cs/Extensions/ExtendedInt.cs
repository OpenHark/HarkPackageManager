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
    public static class ExtendedInt
    {
        public static byte[] GetBytes(this int value)
        {
            return new byte[]
            {
                (byte)((value & 0xFF000000) >> (8 * 3)),
                (byte)((value & 0x00FF0000) >> (8 * 2)),
                (byte)((value & 0x0000FF00) >> (8 * 1)),
                (byte)((value & 0x000000FF) >> (8 * 0))
            };
        }
        
        public static string ToPlural(this int value, string pluralString = "s")
        {
            return value >= 2 ? pluralString : "";
        }
    }   
}