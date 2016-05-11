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
    public static class ExtendedByteArray
    {
        public static string GetString(this byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }
        
        public static int ToInt32(this byte[] bytes)
        {
            int value = 0;
            
            for(uint i = 0; i < 4; ++i)
            {
                value <<= 8;
                value += bytes[i];
            }
            
            return value;
        }
    }   
}