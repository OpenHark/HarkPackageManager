using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PackageUID : UID
    {
        public PackageUID(UID uid)
            : base(uid.RepositoryName, uid.Id)
        { }
    }
    
    public static partial class Extensions
    {
        public static PackageUID ForPackage(this UID uid)
        {
            return new PackageUID(uid);
        }
    }
}