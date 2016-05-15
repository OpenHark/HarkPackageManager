using Hark.HarkPackageManager;

using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Numerics;
using System.Security;
using System.Linq;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public static class ExtendedSecureString
    {
        public static byte[] ToByteArray(this SecureString secureString)
        {
            IntPtr unmanaged = IntPtr.Zero;
            try
            {
                unmanaged = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanaged).GetBytes();
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanaged);
            }
        }
    }
}