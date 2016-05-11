using System.Collections.Generic;
using Hark.HarkPackageManager;

using System.Numerics;

namespace Hark.HarkPackageManager.Library
{
    public interface IPackageVersion
    {
        BigInteger UID
        {
            get;
        }
        
        int Version
        {
            get;
            set;
        }
        bool IsStable
        {
            get;
            set;
        }
        string Description
        {
            get;
            set;
        }
        IPackage Package
        {
            get;
        }
        List<Dependency> Dependencies
        {
            get;
        }
        List<PackageFile> Files
        {
            get;
        }
    }
}