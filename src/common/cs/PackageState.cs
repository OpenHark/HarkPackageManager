using Hark.HarkPackageManager;

using System.IO;

namespace Hark.HarkPackageManager.Library
{
    public enum PackageState : byte
    {
        Release = 0,
        Alpha,
        Beta
    }
    
    public static partial class Extensions
    {
        public static PackageState ReadPackageState(this Stream stream)
        {
            return (PackageState)stream.ReadByte();
        }
        
        public static void Write(this Stream stream, PackageState value)
        {
            stream.WriteByte((byte)value);
        }
    }
}