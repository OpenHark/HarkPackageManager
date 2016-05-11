using Hark.HarkPackageManager.Library;
using Hark.HarkPackageManager;

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace System.Text
{
    public static class ExtendedStringBuilder
    {
        public static char GetLastChar(this StringBuilder sb)
        {
            return sb[sb.Length - 1];
        }
    }   
}