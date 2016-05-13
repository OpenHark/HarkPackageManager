using Hark.HarkPackageManager;

using System.Numerics;
using System.IO;
using System;

namespace Hark.HarkPackageManager.Library
{
    public class PackageVersionUID : UID
    {
        public PackageVersionUID(UID uid)
            : base(uid.RepositoryName, uid.Id)
        { }
    }
    
    public static partial class Extensions
    {
        public static PackageVersionUID ForPackageVersion(this UID uid)
        {
            return new PackageVersionUID(uid);
        }
    }
}